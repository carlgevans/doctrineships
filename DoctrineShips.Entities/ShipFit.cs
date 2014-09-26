namespace DoctrineShips.Entities
{
    using System;
    using System.Collections.Generic;
    using GenericRepository;
    using System.Text;
    using System.Xml;
    
    /// <summary>
    /// A Doctrine Ships fit.
    /// </summary>
    public class ShipFit : EntityBase
    {
        public int ShipFitId { get; set; }
        public int AccountId { get; set; }
        public int HullId { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public string RenderImageUrl { get; set; }
        public string Hull { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int ContractsAvailable { get; set; }
        public bool IsMonitored { get; set; }
        public double FitPackagedVolume { get; set; }
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public double ShippingCost { get; set; }
        public double ContractReward { get; set; }
        public double BuyOrderProfit { get; set; }
        public double SellOrderProfit { get; set; }
        public string FittingString { get; set; }
        public string FittingHash { get; set; }
        public string Notes { get; set; }
        public DateTime LastPriceRefresh { get; set; }
        public virtual ICollection<ShipFitComponent> ShipFitComponents { get; set; }
        public virtual Account Account { get; set; }

        /// <summary>
        /// Contains the strings used to generate xml for slot types.
        /// </summary>
        private Dictionary<SlotType, string> slotToXmlStrings = new Dictionary<SlotType, string>();
        /// <summary>
        /// Contains the slot numbering used when generating xml.
        /// </summary>
        private Dictionary<SlotType, int> slotToXmlCount = new Dictionary<SlotType, int>();

        public string ToXML()
        {
            #region initialize dictionaries
            // This could be moved into a static list so it doesn't need to be recreated every time, but I'm going for clarity over efficiency here.
            slotToXmlStrings.Add(SlotType.Cargo, "cargo");
            slotToXmlStrings.Add(SlotType.Drone, "drone bay");
            slotToXmlStrings.Add(SlotType.High, "hi slot ");
            slotToXmlStrings.Add(SlotType.Medium, "med slot ");
            slotToXmlStrings.Add(SlotType.Low, "low slot ");
            slotToXmlStrings.Add(SlotType.Rig, "rig slot ");
            slotToXmlStrings.Add(SlotType.Subsystem, "subsystem slot ");
            
            slotToXmlCount = new Dictionary<SlotType, int>();
            slotToXmlCount.Add(SlotType.Low, 0);
            slotToXmlCount.Add(SlotType.Medium, 0);
            slotToXmlCount.Add(SlotType.High, 0);
            slotToXmlCount.Add(SlotType.Rig, 0);
            slotToXmlCount.Add(SlotType.Subsystem, 0);
            //TODO: Support for SlotType.Hull and SlotType.Other in either the cargo manner or slot manner

            #endregion

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("\t<fitting name=\"{0}\"/>\r\n", Name);
            sb.AppendFormat("\t\t<description value=\"{0}\"/>\r\n", Role);
            sb.AppendFormat("\t\t<shipType value=\"{0}\"/>\r\n", Hull);

            foreach (ShipFitComponent comp in ShipFitComponents)
            {
                if (slotToXmlCount.ContainsKey(comp.SlotType))
                {
                    for (int qty = 1; qty <= comp.Quantity; qty++)
                    {
                        sb.AppendFormat("\t\t<hardware slot=\"{0}{1}\" type=\"{2}\"/>\r\n", slotToXmlStrings[comp.SlotType], slotToXmlCount[comp.SlotType], comp.Component.Name);
                        slotToXmlCount[comp.SlotType]++;
                    }

                }
                else if (comp.SlotType != SlotType.Hull)
                {
                    sb.AppendFormat("\t\t<hardware qty=\"{0}\" slot=\"{1}\" type=\"{2}\"/>\r\n", comp.Quantity, slotToXmlStrings[comp.SlotType], comp.Component.Name);
                }
            }
            sb.Append("</fitting>");
            return sb.ToString();
        }
    }
}
