namespace DoctrineShips.Service.Managers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Xml.Linq;
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;
    using DoctrineShips.Validation;
    using EveData;
    using GenericRepository;
    using Tools;

    /// <summary>
    /// A class dealing with Doctrine Ships ship fits.
    /// </summary>
    internal sealed class ShipFitManager
    {
        private readonly IDoctrineShipsRepository doctrineShipsRepository;
        private readonly IEveDataSource eveDataSource;
        private readonly IDoctrineShipsValidation doctrineShipsValidation;
        private readonly ISystemLogger logger;

        /// <summary>
        /// Initialises a new instance of a Ship Fit Manager.
        /// </summary>
        /// <param name="doctrineShipsRepository">An IDoctrineShipsRepository instance.</param>
        /// <param name="eveDataSource">An IEveDataSource instance.</param>
        /// <param name="doctrineShipsValidation">An IDoctrineShipsValidation instance.</param>
        /// <param name="logger">An ISystemLogger logger instance.</param>
        internal ShipFitManager(IDoctrineShipsRepository doctrineShipsRepository, IEveDataSource eveDataSource, IDoctrineShipsValidation doctrineShipsValidation, ISystemLogger logger)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
            this.eveDataSource = eveDataSource;
            this.doctrineShipsValidation = doctrineShipsValidation;
            this.logger = logger;
        }

        /// <summary>
        /// Fetches and returns a Doctrine Ships ship fit.
        /// </summary>
        /// <param name="shipFitId">The id for which a ship fit object should be returned.</param>
        /// <param name="settingProfile">A doctrine ships account setting profile.</param>
        /// <param name="accountId">The currently logged-in account id for security checking.</param>
        /// <returns>A ship fit object.</returns>
        internal ShipFit GetShipFitDetail(int shipFitId, int accountId, SettingProfile settingProfile)
        {
            ShipFit shipFit = this.doctrineShipsRepository.GetShipFit(shipFitId);

            if (shipFit != null)
            {
                if (accountId == shipFit.AccountId)
                {
                    // Refresh the market details for the ship fit.
                    this.RefreshShipFitMarketDetails(shipFit, settingProfile);

                    return shipFit;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a list of all Doctrine Ships ship fits for a given account.
        /// </summary>
        /// <param name="accountId">The account for which the ship fits should be returned.</param>
        /// <returns>A list of ship fit objects.</returns>
        internal IEnumerable<ShipFit> GetShipFitList(int accountId)
        {
            return this.doctrineShipsRepository.GetShipFitsForAccount(accountId);
        }

        /// <summary>
        /// Returns a list of all doctrines for a given account.
        /// </summary>
        /// <param name="accountId">The account for which the doctrines should be returned.</param>
        /// <returns>A list of doctrine objects.</returns>
        internal IEnumerable<Doctrine> GetDoctrineList(int accountId)
        {
            return this.doctrineShipsRepository.GetDoctrinesForAccount(accountId);
        }

        /// <summary>
        /// Returns a doctrine for a given account and doctrine id.
        /// </summary>
        /// <param name="accountId">The currently logged-in account id for security checking.</param>
        /// <param name="doctrineId">The id for which a doctrine object should be returned.</param>
        /// <returns>A doctrine object.</returns>
        internal Doctrine GetDoctrineDetail(int accountId, int doctrineId)
        {
            Doctrine doctrine = this.doctrineShipsRepository.GetDoctrine(doctrineId);

            if (doctrine != null)
            {
                if (accountId == doctrine.AccountId)
                {
                    return doctrine;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a list of contracts for a given ship fit.
        /// </summary>
        /// <param name="shipFitId">The id of the ship fit for which contracts should be returned.</param>
        /// <returns>A list of ship fit contract objects.</returns>
        internal IEnumerable<Contract> GetShipFitContracts(int shipFitId)
        {
            return this.doctrineShipsRepository.GetShipFitContracts(shipFitId);
        }

        /// <summary>
        /// Deletes a ship fit, its components and all related contracts.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the ship fit being deleted.</param>
        /// <param name="shipFitId">The ship fit Id to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        internal bool DeleteShipFit(int accountId, int shipFitId)
        {
            ShipFit shipFit = this.doctrineShipsRepository.GetShipFit(shipFitId);

            if (shipFit != null)
            {
                // If the account Id matches the account Id of the ship fit being deleted, continue.
                if (accountId == shipFit.AccountId)
                {
                    // Delete any contracts matching this ship fit id.
                    this.doctrineShipsRepository.DeleteContractsByShipFitId(shipFit.ShipFitId);

                    // Delete the ship fit from any doctrines.
                    this.doctrineShipsRepository.DeleteDoctrineShipFitsByShipFitId(shipFit.ShipFitId);

                    // Delete all of the ship fit's components.
                    this.doctrineShipsRepository.DeleteShipFitComponentsByShipFitId(shipFit.ShipFitId);
                    
                    // Delete the ship fit.
                    this.doctrineShipsRepository.DeleteShipFit(shipFit.ShipFitId);

                    // Save and log the event.
                    this.doctrineShipsRepository.Save();
                    logger.LogMessage("Ship Fit '" + shipFit.Name + "' Successfully Deleted For Account Id: " + shipFit.AccountId, 1, "Message", MethodBase.GetCurrentMethod().Name);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Imports a Doctrine Ships ship fit from a string formatted in the standard CCP Xml format.
        /// </summary>
        /// <param name="toParse">A ship fitting string formatted in the standard CCP Xml format.</param>
        /// <param name="accountId">The account for which the ship fit should be imported.</param>
        /// <returns>Returns a list of populated ship fit objects or an empty list if the process fails.</returns>
        internal IEnumerable<ShipFit> ImportShipFitEveXml(string toParse, int accountId)
        {
            ICollection<ShipFit> shipFits = new List<ShipFit>();
            XDocument xmlDoc;

            // Does the account exist?
            if (this.doctrineShipsRepository.GetAccount(accountId) != null)
            {
                try
                {
                    using (StringReader stringReader = new StringReader(toParse))
                    {
                        xmlDoc = XDocument.Load(stringReader);
                    }

                    foreach (XElement fitting in xmlDoc.Root.Elements("fitting"))
                    {
                        // Read and cleanse the initial xml elements containing the fitting name, description and ship type.
                        string shipFitName = String.Empty;
                        string shipFitHull = String.Empty;
                        string shipFitRole = String.Empty;

                        shipFitName = Conversion.StringToSafeString((string)fitting.Attribute("name") ?? String.Empty);

                        if (fitting.Element("shipType") != null)
                        {
                            shipFitHull = Conversion.StringToSafeString((string)fitting.Element("shipType").Attribute("value") ?? String.Empty);
                        }

                        if (fitting.Element("description") != null)
                        {
                            shipFitRole = Conversion.StringToSafeString((string)fitting.Element("description").Attribute("value") ?? String.Empty);
                        }

                        // Attempt to populate, validate and add a new ship fit to the database.
                        ShipFit newShipFit = this.AddShipFit(shipFitName, shipFitRole, shipFitHull, accountId);

                        // If the ship fit addition was successful, continue with the parse.
                        if (newShipFit != null)
                        {
                            // Add the ship fit to the list of successfully imported ship fits.
                            shipFits.Add(newShipFit);

                            // Instantiate a new list of ShipFitComponents.
                            ICollection<ShipFitComponent> shipFitComponents = new List<ShipFitComponent>();

                            // Add the ship hull itself to the list of ShipFitComponents.
                            this.AddComponent(newShipFit.Hull);
                            shipFitComponents.Add(new ShipFitComponent
                            {
                                ComponentId = newShipFit.HullId,
                                ShipFitId = newShipFit.ShipFitId,
                                SlotType = SlotType.Hull,
                                Quantity = 1
                            });

                            // Iterate through the 'hardware' xml elements containing the ship fitting components.
                            foreach (XElement component in fitting.Elements("hardware"))
                            {
                                // Attempt to add this parsed component to the database.
                                Component newComponent = this.AddComponent((string)component.Attribute("type") ?? String.Empty);

                                // Unless the component was not recognised, continue with the parse.
                                if (newComponent != null)
                                {
                                    ShipFitComponent newShipFitComponent = new ShipFitComponent();

                                    newShipFitComponent.ComponentId = newComponent.ComponentId;
                                    newShipFitComponent.ShipFitId = newShipFit.ShipFitId;

                                    switch (component.Attribute("slot").Value.Split(' ').FirstOrDefault())
                                    {
                                        case "hi":
                                            newShipFitComponent.SlotType = SlotType.High;
                                            break;
                                        case "med":
                                            newShipFitComponent.SlotType = SlotType.Medium;
                                            break;
                                        case "low":
                                            newShipFitComponent.SlotType = SlotType.Low;
                                            break;
                                        case "rig":
                                            newShipFitComponent.SlotType = SlotType.Rig;
                                            break;
                                        case "subsystem":
                                            newShipFitComponent.SlotType = SlotType.Subsystem;
                                            break;
                                        case "drone":
                                            newShipFitComponent.SlotType = SlotType.Drone;
                                            break;
                                        case "cargo":
                                            newShipFitComponent.SlotType = SlotType.Cargo;
                                            break;
                                        default:
                                            newShipFitComponent.SlotType = SlotType.Other;
                                            break;
                                    }

                                    if (component.Attribute("qty") != null)
                                    {
                                        newShipFitComponent.Quantity = Conversion.StringToLong(component.Attribute("qty").Value);
                                    }
                                    else
                                    {
                                        newShipFitComponent.Quantity = 1;
                                    }

                                    // Add this component to the list.
                                    shipFitComponents.Add(newShipFitComponent);
                                }
                            }

                            // Add the full list of components to the ship fit.
                            this.AddShipFitComponents(shipFitComponents);

                            // Now that all of the ship fit components have been added, refresh the total volume of the fit.
                            this.RefreshShipFitPackagedVolume(newShipFit);

                            // Generate and refresh the fitting string for the ship fit.
                            this.RefreshFittingString(newShipFit);

                            // Generate a fitting hash unique to this account.
                            this.RefreshFittingHash(newShipFit, accountId);
                        }
                    }
                }
                catch (System.FormatException e)
                {
                    this.logger.LogMessage("An error occured while parsing the fitting Xml. Is the Xml format correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                    this.logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
                }
                catch (System.ArgumentException e)
                {
                    this.logger.LogMessage("An error occured while parsing the fitting Xml. Is the Xml format correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                    this.logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
                }
                catch (System.Xml.XmlException e)
                {
                    this.logger.LogMessage("An error occured while parsing the fitting Xml. Is the Xml format correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                    this.logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return shipFits;
        }

        /// <summary>
        /// Adds a Doctrine Ships ship fit.
        /// </summary>
        /// <param name="name">The name of the ship fit. E.g. 'L33t Hax0r Brutix'</param>
        /// <param name="role">The role of the ship fit. E.g. 'DPS'</param>
        /// <param name="hull">The type of the hull. E.g. 'Brutix'</param>
        /// <param name="accountId">The account for which the ship fit should be added.</param>
        /// <returns>Returns a populated ship fit object or null if the process fails.</returns>
        internal ShipFit AddShipFit(string name, string role, string hull, int accountId)
        {
            int hullId = 0;
            ShipFit shipFit = null;

            // Does the hull name successfully resolve to an id? 
            hullId = eveDataSource.GetTypeId(hull);

            if (hullId != 0)
            {
                shipFit = new ShipFit();

                shipFit.HullId = hullId;

                // Resolve the hull id back to the hull name to ensure that it is correct.
                shipFit.Hull = eveDataSource.GetTypeName(hullId);
                shipFit.AccountId = accountId;
                shipFit.ThumbnailImageUrl = eveDataSource.GetTypeImageUrl(hullId, 64);
                shipFit.RenderImageUrl = eveDataSource.GetTypeRenderImageUrl(hullId);
                shipFit.Name = name;
                shipFit.Role = role;
                shipFit.ContractsAvailable = 0;
                shipFit.IsMonitored = false;
                shipFit.FitPackagedVolume = 0;
                shipFit.BuyPrice = 0;
                shipFit.SellPrice = 0;
                shipFit.ShippingCost = 0;
                shipFit.ContractReward = 0;
                shipFit.BuyOrderProfit = 0;
                shipFit.SellOrderProfit = 0;
                shipFit.FittingString = string.Empty;
                shipFit.FittingHash = string.Empty;
                shipFit.Notes = string.Empty;
                shipFit.LastPriceRefresh = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));

                if (this.doctrineShipsValidation.ShipFit(shipFit).IsValid == true)
                {
                    // Add the new ship fit and read it back to get the auto generated primary key id.
                    shipFit = this.doctrineShipsRepository.CreateShipFit(shipFit);
                    this.doctrineShipsRepository.Save();
                }
            }

            return shipFit;
        }

        /// <summary>
        /// <para>Adds a ship fit component from a list of partially populated ship fit components.</para>
        /// <para>The minimum properties that should be populated:</para>
        /// <para>-ShipFitId</para>
        /// <para>-ComponentId</para>
        /// <para>-Quantity</para>
        /// <para>-Slot</para>
        /// </summary>
        /// <param name="componentList">A list of partially populated ship fit components.</param>
        /// <returns>Returns a list of populated component objects or an empty list if the process fails.</returns>
        internal IEnumerable<ShipFitComponent> AddShipFitComponents(IEnumerable<ShipFitComponent> componentList)
        {
            ICollection<ShipFitComponent> shipFitComponents = new List<ShipFitComponent>();

            if (componentList != null && componentList.Count() != 0)
            {
                var compressedComponentList = CompressShipFitComponents(componentList);

                if (compressedComponentList != null && compressedComponentList.Count() != 0)
                {
                    foreach (var component in compressedComponentList)
                    {
                        ShipFitComponent shipFitComponent = new ShipFitComponent();

                        shipFitComponent.ShipFitId = component.ShipFitId;
                        shipFitComponent.ComponentId = component.ComponentId;
                        shipFitComponent.Quantity = component.Quantity;
                        shipFitComponent.SlotType = component.SlotType;
                        shipFitComponent.BuyPrice = 0;
                        shipFitComponent.SellPrice = 0;

                        // Validate the new ship fit component.
                        if (this.doctrineShipsValidation.ShipFitComponent(shipFitComponent).IsValid == true)
                        {
                            // Add the populated ship fit component object to the database and read it back to get the auto generated primary key id.
                            shipFitComponent = this.doctrineShipsRepository.CreateShipFitComponent(shipFitComponent);

                            // Add the ship fit component to the return list.
                            shipFitComponents.Add(shipFitComponent);
                        }
                    }

                    // Commit changes to the database.
                    this.doctrineShipsRepository.Save();
                }
            }

            return shipFitComponents;
        }

        /// <summary>
        /// Adds a component based upon its name.
        /// </summary>
        /// <param name="componentName">The name of a component to be added.</param>
        /// <returns>Returns a populated component object or null if the process fails.</returns>
        internal Component AddComponent(string componentName)
        {
            int componentId = 0;
            Component component = null;

            // Does the component name successfully resolve to an id? 
            componentId = eveDataSource.GetTypeId(componentName);

            if (componentId != 0)
            {
                // Does the component already exist in the database? If not, add it.
                component = this.doctrineShipsRepository.GetComponent(componentId);

                if (component == null)
                {
                    component = new Component();

                    component.ComponentId = componentId;

                    // Resolve the id back to the name to get correct capitalisation etc 
                    component.Name = eveDataSource.GetTypeName(componentId);
                    component.ImageUrl = eveDataSource.GetTypeImageUrl(componentId);
                    component.Volume = eveDataSource.GetTypeVolume(componentId);

                    // Validate the new component.
                    if (this.doctrineShipsValidation.Component(component).IsValid == true)
                    {
                        // Add the new component and read it back.
                        component = this.doctrineShipsRepository.CreateComponent(component);
                        this.doctrineShipsRepository.Save();
                    }
                }
            }

            return component;
        }

        /// <summary>
        /// Compresses a list of ship fit components, grouping duplicates (excluding different slots) but adding their quantities together.
        /// </summary>
        /// <param name="componentList">A list of ship fit components.</param>
        /// <returns>Returns a compressed list of ship fit components.</returns>
        internal IEnumerable<ShipFitComponent> CompressShipFitComponents(IEnumerable<ShipFitComponent> componentList)
        {
            IEnumerable<ShipFitComponent> compressedComponentList;

            compressedComponentList = componentList
                .OrderBy(o => o.ComponentId)
                .GroupBy(u => new { u.ComponentId, u.SlotType })
                .Select(x => new ShipFitComponent()
                {
                    ComponentId = x.Key.ComponentId,
                    SlotType = x.Key.SlotType,
                    ShipFitId = x.FirstOrDefault().ShipFitId,
                    Quantity = x.Sum(p => p.Quantity)
                });

            return compressedComponentList;
        }

        /// <summary>
        /// Refresh the number of valid contracts available for each ship fit.
        /// </summary>
        internal void RefreshShipFitContractCounts()
        {
            // Fetch all ship fits where contract counting is enabled (>=0).
            IEnumerable<ShipFit> shipFits;
            shipFits = this.doctrineShipsRepository.GetShipFitsForContractCount();

            if (shipFits.Any() == true)
            {
                // Fetch a dictionary of ship fit ids and their numbers of valid and outstanding item exchange contracts.
                Dictionary<int, int> contracts = this.doctrineShipsRepository.GetContractShipFitCounts();
                int quantity;

                foreach (var shipFit in shipFits)
                {
                    contracts.TryGetValue(shipFit.ShipFitId, out quantity);
                    shipFit.ContractsAvailable = quantity;

                    // Update the contract count value for the current ship fit in the database.
                    this.doctrineShipsRepository.UpdateShipFit(shipFit);
                }

                // Commit the changes to the database.
                this.doctrineShipsRepository.Save();
            }
        }

        /// <summary>
        /// Refresh the packaged volume for a ship fit.
        /// </summary>
        /// <param name="shipFit">A doctrine ships ship fit.</param>
        internal void RefreshShipFitPackagedVolume(ShipFit shipFit)
        {
            shipFit.FitPackagedVolume = 0;

            // Ensure that there are ship fit components to be refreshed.
            if (shipFit.ShipFitComponents != null && shipFit.ShipFitComponents.Any() != false)
            {
                foreach (var item in shipFit.ShipFitComponents)
                {
                    // Add the value to the shipfit's total volume.
                    shipFit.FitPackagedVolume += item.Component.Volume * item.Quantity;
                }

                // Update the ship fit in the database.
                this.doctrineShipsRepository.UpdateShipFit(shipFit);

                // Commit changes to the database.
                this.doctrineShipsRepository.Save();
            }
        }

        /// <summary>
        /// Refresh the market details for a ship fit.
        /// </summary>
        /// <param name="shipFit">A doctrine ships ship fit.</param>
        /// <param name="settingProfile">A doctrine ships account setting profile.</param>
        internal void RefreshShipFitMarketDetails(ShipFit shipFit, SettingProfile settingProfile)
        {
            // Only update the ship fit prices if they are older than 5 minutes.
            if (Time.HasElapsed(shipFit.LastPriceRefresh, TimeSpan.FromMinutes(5)) == true)
            {
                // Update the buy and sell prices of all ship fit components.
                this.RefreshShipFitComponentsPrices(shipFit, settingProfile);

                // Ship Fit Calculations.
                shipFit.BuyPrice = shipFit.ShipFitComponents.Sum(x => (x.BuyPrice * x.Quantity));
                shipFit.SellPrice = shipFit.ShipFitComponents.Sum(x => (x.SellPrice * x.Quantity));
                shipFit.ShippingCost = shipFit.FitPackagedVolume * settingProfile.ShippingCostPerM3;
                shipFit.ContractReward = (shipFit.SellPrice * settingProfile.ContractMarkupPercentage) + shipFit.ShippingCost + settingProfile.ContractBrokerFee;
                shipFit.BuyOrderProfit = shipFit.ContractReward - shipFit.BuyPrice;
                shipFit.SellOrderProfit = shipFit.ContractReward - shipFit.SellPrice;

                // Set the ship fit's last price refresh timestamp to the current date & time.
                shipFit.LastPriceRefresh = DateTime.UtcNow;

                // Update the ship fit in the database.
                this.doctrineShipsRepository.UpdateShipFit(shipFit);

                // Commit changes to the database.
                this.doctrineShipsRepository.Save();
            }
        }

        /// <summary>
        /// Refresh the market prices of all components in a ship fit.
        /// </summary>
        /// <param name="shipFit">A doctrine ships ship fit.</param>
        /// <param name="settingProfile">A doctrine ships account setting profile.</param>
        internal void RefreshShipFitComponentsPrices(ShipFit shipFit, SettingProfile settingProfile)
        {
            double buyPrice;
            double sellPrice;
            IDictionary<int, double> buyPrices;
            IDictionary<int, double> sellPrices;

            // Ensure that there are ship fit components to be refreshed.
            if (shipFit.ShipFitComponents != null && shipFit.ShipFitComponents.Any() != false)
            {
                buyPrices = this.eveDataSource.GetStationBuyPrice(shipFit.ShipFitComponents.Select(x => x.ComponentId).ToList(), settingProfile.BuyStationId);
                sellPrices = this.eveDataSource.GetStationSellPrice(shipFit.ShipFitComponents.Select(x => x.ComponentId).ToList(), settingProfile.SellStationId);

                foreach (var item in shipFit.ShipFitComponents)
                {
                    buyPrices.TryGetValue(item.ComponentId, out buyPrice);
                    sellPrices.TryGetValue(item.ComponentId, out sellPrice);

                    item.BuyPrice = buyPrice * settingProfile.BrokerPercentage;
                    item.SellPrice = sellPrice;

                    item.ObjectState = ObjectState.Modified;
                }
            }
        }

        /// <summary>
        /// Generate and refresh the EVE IGB fitting string for a ship fit.
        /// </summary>
        /// <param name="shipFit">A doctrine ships ship fit.</param>
        internal void RefreshFittingString(ShipFit shipFit)
        {
            string fittingString = string.Empty;

            // Generate the fitting string.
            fittingString += shipFit.HullId + ":";

            foreach (var item in shipFit.ShipFitComponents)
            {
                fittingString += item.ComponentId + ";" + item.Quantity + ":";
            }

            fittingString += ":";

            // Update the fitting string and ship fit in the database.
            shipFit.FittingString = fittingString;
            this.doctrineShipsRepository.UpdateShipFit(shipFit);

            // Commit changes to the database.
            this.doctrineShipsRepository.Save();
        }

        /// <summary>
        /// Generate and refresh the fitting hash, unique to an account.
        /// </summary>
        /// <param name="shipFit">A doctrine ships ship fit.</param>
        /// <param name="accountId">The account Id for which a fitting hash should be generated.</param>
        internal void RefreshFittingHash(ShipFit shipFit, int accountId)
        {
            string concatComponents = string.Empty;
            IEnumerable<ShipFitComponent> compressedShipFitComponents = new List<ShipFitComponent>();

            if (shipFit.ShipFitComponents != null && shipFit.ShipFitComponents.Any() == true)
            {
                // Compress the ship fit components list, removing duplicates but adding the quantities.
                compressedShipFitComponents = shipFit.ShipFitComponents
                        .OrderBy(o => o.ComponentId)
                        .GroupBy(u => u.ComponentId)
                        .Select(x => new ShipFitComponent()
                        {
                            ComponentId = x.Key,
                            Quantity = x.Sum(p => p.Quantity)
                        });

                // Concatenate all components and their quantities into a single string.
                foreach (var item in compressedShipFitComponents)
                {
                    concatComponents += item.ComponentId + item.Quantity;
                }

                // Generate a hash for the fitting and salt it with the account id. This permits identical fits across accounts.
                shipFit.FittingHash = Security.GenerateHash(concatComponents, accountId.ToString());
            }
            else
            {
                // There are no components so generate a random hash.
                shipFit.FittingHash = Security.GenerateHash(Security.GenerateRandomString(32), Security.GenerateSalt(6));
            }

            // Commit changes to the database.
            this.doctrineShipsRepository.UpdateShipFit(shipFit);
            this.doctrineShipsRepository.Save();
        }

        /// <summary>
        /// Generate and returns an EFT fitting string for a ship fit.
        /// </summary>
        /// <param name="shipFitId">The id of a doctrine ships ship fit.</param>
        /// <param name="accountId">The currently logged-in account id for security checking.</param>
        /// <returns>Returns a string containing an EFT fitting or an empty string if an error occurs.</returns>
        internal string GetEftFittingString(int shipFitId, int accountId)
        {
            string eftFitting = string.Empty;

            ShipFit shipFit = this.doctrineShipsRepository.GetShipFit(shipFitId);

            if (shipFit != null)
            {
                if (accountId == shipFit.AccountId)
                {
                    // Generate the fitting string.
                    eftFitting += "[" + shipFit.Hull + ", " + shipFit.Name + "]" + Environment.NewLine;

                    foreach (var item in shipFit.ShipFitComponents.OrderBy(x => x.SlotType))
                    {
                        if (item.SlotType == SlotType.Cargo || item.SlotType == SlotType.Drone)
                        {
                            // This is a cargo or drone slot, so append the quantity.
                            eftFitting += item.Component.Name + " x" + item.Quantity + Environment.NewLine;
                        }
                        else if (item.SlotType != SlotType.Hull)
                        {
                            for (int i = 1; i <= item.Quantity; i++)
                            {
                                // This is a module, so add each of the items as a separate line.
                                eftFitting += item.Component.Name + Environment.NewLine;
                            }
                        }
                    }
                }
            }

            return eftFitting;
        }

        /// <summary>
        /// Generate and refresh all fitting strings.
        /// </summary>
        internal void RefreshAllFittingStrings()
        {
            IEnumerable<ShipFit> shipFits = this.doctrineShipsRepository.GetShipFitsWithComponents();

            if (shipFits.Any() == true)
            {
                foreach (var shipFit in shipFits)
                {
                    this.RefreshFittingString(shipFit);
                }
            }
        }

        /// <summary>
        /// Generate and refresh all fitting hashes.
        /// </summary>
        internal void RefreshAllFittingHashes()
        {
            IEnumerable<ShipFit> shipFits = this.doctrineShipsRepository.GetShipFitsWithComponents();

            if (shipFits.Any() == true)
            {
                foreach (var shipFit in shipFits)
                {
                    this.RefreshFittingHash(shipFit, shipFit.AccountId);
                }
            }
        }

        /// <summary>
        /// Refresh packaged volumes for all ship fits.
        /// </summary>
        internal void RefreshAllShipFitPackagedVolumes()
        {
            IEnumerable<ShipFit> shipFits = this.doctrineShipsRepository.GetShipFitsWithComponents();

            if (shipFits.Any() == true)
            {
                foreach (var shipFit in shipFits)
                {
                    this.RefreshShipFitPackagedVolume(shipFit);
                }
            }
        }

        /// <summary>
        /// Updates the state of a ship fit's contract availability monitoring.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the sales agent being changed.</param>
        /// <param name="shipFitId">The id of the ship fit to be changed.</param>
        /// <param name="isActive">The required boolean state.</param>
        /// <returns>Returns true if the change was successful or false if not.</returns>
        internal bool UpdateShipFitMonitoringState(int accountId, int shipFitId, bool isActive)
        {
            ShipFit shipFit = this.doctrineShipsRepository.GetShipFit(shipFitId);

            if (shipFit != null)
            {
                // If the account Id matches the account Id of the ship fit being changed, continue.
                if (accountId == shipFit.AccountId)
                {
                    // Change the state of the ship fit and log the event.
                    if (isActive == true)
                    {
                        shipFit.IsMonitored = true;
                    }
                    else
                    {
                        shipFit.IsMonitored = false;
                    }

                    this.doctrineShipsRepository.UpdateShipFit(shipFit);
                    this.doctrineShipsRepository.Save();

                    if (isActive == true)
                    {
                        logger.LogMessage("Contract Availability Monitoring For Ship Fit '" + shipFit.Name + "' Successfully Enabled. Account Id: " + shipFit.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name);
                    }
                    else
                    {
                        logger.LogMessage("Contract Availability Monitoring For Ship Fit '" + shipFit.Name + "' Successfully Disabled. Account Id: " + shipFit.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name);
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Updates a ship fit for a particular account.
        /// </summary>
        /// <param name="shipFit">A partially populated ship fit object to be updated.</param>
        /// <returns>Returns a validation result object.</returns>
        internal IValidationResult UpdateShipFit(ShipFit shipFit)
        {
            IValidationResult validationResult = new ValidationResult();

            var existingShipFit = this.doctrineShipsRepository.GetShipFit(shipFit.ShipFitId);

            if (existingShipFit != null)
            {
                if (existingShipFit.AccountId != shipFit.AccountId)
                {
                    validationResult.AddError("ShipFit.Permission", "The ship fit being modified does not belong to the requesting account.");
                }
                else
                {
                    // Map the updates to the existing ship fit.
                    existingShipFit.Name = shipFit.Name;
                    existingShipFit.Role = shipFit.Role;
                    existingShipFit.Notes = shipFit.Notes;

                    // Validate the ship fit updates.
                    validationResult = this.doctrineShipsValidation.ShipFit(existingShipFit);
                    if (validationResult.IsValid == true)
                    {
                        // Update the existing record, save and log.
                        this.doctrineShipsRepository.UpdateShipFit(existingShipFit);
                        this.doctrineShipsRepository.Save();
                        logger.LogMessage("Ship Fit '" + existingShipFit.Name + "' Successfully Updated For Account Id: " + existingShipFit.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name);
                    }
                }
            }

            return validationResult;
        }

        /// <summary>
        /// Deletes a doctrine.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the doctrine being deleted.</param>
        /// <param name="doctrineId">The doctrine Id to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        internal bool DeleteDoctrine(int accountId, int doctrineId)
        {
            Doctrine doctrine = this.doctrineShipsRepository.GetDoctrine(doctrineId);

            if (doctrine != null)
            {
                // If the account Id matches the account Id of the doctrine being deleted, continue.
                if (accountId == doctrine.AccountId)
                {
                    // Delete all of the doctrine ship fits.
                    this.doctrineShipsRepository.DeleteDoctrineShipFitsByDoctrineId(doctrine.DoctrineId);

                    // Delete the doctrine and log the event.
                    this.doctrineShipsRepository.DeleteDoctrine(doctrine.DoctrineId);
                    this.doctrineShipsRepository.Save();
                    logger.LogMessage("Doctrine '" + doctrine.Name + "' Successfully Deleted For Account Id: " + doctrine.AccountId, 1, "Message", MethodBase.GetCurrentMethod().Name);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// <para>Adds a Doctrine.</para>
        /// </summary>
        /// <param name="doctrine">A populated doctrine object.</param>
        /// <returns>Returns a validation result object.</returns>
        internal IValidationResult AddDoctrine(Doctrine doctrine)
        {
            IValidationResult validationResult = new ValidationResult();

            doctrine.LastUpdate = DateTime.UtcNow;

            // Validate the new doctrine.
            validationResult = this.doctrineShipsValidation.Doctrine(doctrine);
            if (validationResult.IsValid == true)
            {
                // Add the new doctrine.
                this.doctrineShipsRepository.CreateDoctrine(doctrine);
                this.doctrineShipsRepository.Save();

                // Log the addition.
                logger.LogMessage("Doctrine '" + doctrine.Name + "' Successfully Added.", 2, "Message", MethodBase.GetCurrentMethod().Name);
            }

            return validationResult;
        }

        /// <summary>
        /// Updates a doctrine for a particular account.
        /// </summary>
        /// <param name="doctrine">A partially populated doctrine object to be updated.</param>
        /// <returns>Returns a validation result object.</returns>
        internal IValidationResult UpdateDoctrine(Doctrine doctrine)
        {
            IValidationResult validationResult = new ValidationResult();

            var existingDoctrine = this.doctrineShipsRepository.GetDoctrine(doctrine.DoctrineId);

            if (existingDoctrine != null)
            {
                if (existingDoctrine.AccountId != doctrine.AccountId)
                {
                    validationResult.AddError("Doctrine.Permission", "The doctrine being modified does not belong to the requesting account.");
                }
                else
                {
                    // Map the updates to the existing doctrine.
                    existingDoctrine.Name = doctrine.Name;
                    existingDoctrine.Description = doctrine.Description;
                    existingDoctrine.ImageUrl = doctrine.ImageUrl;
                    existingDoctrine.IsOfficial = doctrine.IsOfficial;
                    existingDoctrine.IsDormant = doctrine.IsDormant;
                    existingDoctrine.LastUpdate = DateTime.UtcNow;

                    // Validate the doctrine updates.
                    validationResult = this.doctrineShipsValidation.Doctrine(existingDoctrine);
                    if (validationResult.IsValid == true)
                    {
                        // Update the existing record, save and log.
                        this.doctrineShipsRepository.UpdateDoctrine(existingDoctrine);
                        this.doctrineShipsRepository.Save();
                        logger.LogMessage("Doctrine '" + existingDoctrine.Name + "' Successfully Updated For Account Id: " + existingDoctrine.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name);
                    }
                }
            }

            return validationResult;
        }

        /// <summary>
        /// Updates a doctrine ship fit list for a particular account.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the doctrine being updated.</param>
        /// <param name="doctrineId">The doctrine Id to be updated.</param>
        /// <param name="doctrineShipFitIds">An array of ship fit ids to be assigned to the doctrine.</param>
        /// <returns>Returns a validation result object.</returns>
        internal IValidationResult UpdateDoctrineShipFits(int accountId, int doctrineId, int[] doctrineShipFitIds)
        {
            IValidationResult validationResult = new ValidationResult();

            var existingDoctrine = this.doctrineShipsRepository.GetDoctrine(doctrineId);

            if (existingDoctrine != null)
            {
                if (existingDoctrine.AccountId != accountId)
                {
                    validationResult.AddError("Doctrine.Permission", "The doctrine ship fit list being modified does not belong to the requesting account.");
                }
                else
                {
                    // Delete all existing ship fits for this doctrine.
                    this.doctrineShipsRepository.DeleteDoctrineShipFitsByDoctrineId(doctrineId);

                    if (doctrineShipFitIds != null)
                    {
                        // Create a new entry for each ship fit id that was passed.
                        foreach (int shipFitId in doctrineShipFitIds)
                        {
                            DoctrineShipFit doctrineShipFit = new DoctrineShipFit();

                            doctrineShipFit.DoctrineId = doctrineId;
                            doctrineShipFit.ShipFitId = shipFitId;

                            // Add the new doctrine ship fit.
                            doctrineShipFit = this.doctrineShipsRepository.CreateDoctrineShipFit(doctrineShipFit);
                        }
                    }

                    // Update the last update timestamp of the doctrine.
                    existingDoctrine.LastUpdate = DateTime.UtcNow;
                    this.doctrineShipsRepository.UpdateDoctrine(existingDoctrine);

                    // Save the changes to the database and log.
                    this.doctrineShipsRepository.Save();
                    logger.LogMessage("Ship Fit List For Doctrine '" + existingDoctrine.Name + "' Successfully Updated For Account Id: " + existingDoctrine.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name);
                }
            }

            return validationResult;
        }
    }
}
