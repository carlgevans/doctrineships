namespace DoctrineShips.Test
{
    using System;
    using DoctrineShips.Data;
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;
    using DoctrineShips.Service;
    using DoctrineShips.Validation;
    using DoctrineShips.Validation.Checks;
    using DoctrineShips.Web.Controllers;
    using EveData;
    using GenericRepository;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tools;
    using DoctrineShips.Service.Entities;
    using System.Web.Mvc;

    [TestClass]
    public class ContractCheckTests
    {
        private readonly DateTime dateNow;
        private readonly Controller controller;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEveDataSource eveDataSource;
        private readonly IDoctrineShipsRepository doctrineShipsRepository;
        private readonly IDoctrineShipsServices doctrineShipsServices;
        private readonly IDoctrineShipsValidation doctrineShipsValidation;
        private readonly ISystemLogger logger;
        private readonly ISystemLoggerStore loggerStore;
        private readonly ContractCheck contractCheck;

        public ContractCheckTests()
        {
            this.dateNow = DateTime.UtcNow;
            this.doctrineShipsServices = new DoctrineShipsServices(this.doctrineShipsRepository, this.eveDataSource, this.doctrineShipsValidation, null, this.logger);
            this.controller = new SearchController(this.doctrineShipsServices);
            this.unitOfWork = new UnitOfWork(new DoctrineShipsContext());
            this.eveDataSource = new EveDataSourceCached(this.logger);
            this.doctrineShipsRepository = new DoctrineShipsRepository(this.unitOfWork);
            this.contractCheck = new ContractCheck(this.doctrineShipsRepository);
            this.doctrineShipsValidation = new DoctrineShipsValidation(this.doctrineShipsRepository);
            this.loggerStore = new DoctrineShipsRepository(this.unitOfWork);
            this.logger = new SystemLogger(this.loggerStore);
        }

        ~ContractCheckTests()
        {
            this.unitOfWork.Dispose();
        }

        [TestMethod]
        public void IsValidContract_PassValidContract_ReturnTrue()
        {
            Contract contract = new Contract()
            {
                AssigneeId = 234,
                Availability = ContractAvailability.Private,
                ContractId = 1,
                DateExpired = dateNow,
                DateIssued = dateNow,
                ForCorp = false,
                IssuerCorpId = 567,
                IssuerId = 890,
                Price = 123,
                ShipFitId = 1,
                SolarSystemId = 123123,
                SolarSystemName = "Solar System Name",
                StartStationId = 321321,
                StartStationName = "Start Station Name",
                Status = ContractStatus.Outstanding,
                Title = "",
                Type = ContractType.ItemExchange,
                Volume = 100
            };

            Assert.IsTrue(contractCheck.Contract(contract).IsValid);
        }

        [TestMethod]
        public void IsValidContract_PassContractId0_ReturnFalse()
        {
            Contract contract = new Contract()
            {
                AssigneeId = 234,
                Availability = ContractAvailability.Private,
                ContractId = 0,
                DateExpired = dateNow,
                DateIssued = dateNow,
                ForCorp = false,
                IssuerCorpId = 567,
                IssuerId = 890,
                Price = 123,
                ShipFitId = 1,
                SolarSystemId = 123123,
                SolarSystemName = "Solar System Name",
                StartStationId = 321321,
                StartStationName = "Start Station Name",
                Status = ContractStatus.Outstanding,
                Title = "",
                Type = ContractType.ItemExchange,
                Volume = 100
            };

            Assert.IsFalse(contractCheck.Contract(contract).IsValid);
        }

        [TestMethod]
        public void IsValidContract_PassContractIdAlreadyExists_ReturnFalse()
        {
            Contract contract = new Contract()
            {
                AssigneeId = 234,
                Availability = ContractAvailability.Private,
                ContractId = 75580554,
                DateExpired = dateNow,
                DateIssued = dateNow,
                ForCorp = false,
                IssuerCorpId = 567,
                IssuerId = 890,
                Price = 123,
                ShipFitId = 1,
                SolarSystemId = 123123,
                SolarSystemName = "Solar System Name",
                StartStationId = 321321,
                StartStationName = "Start Station Name",
                Status = ContractStatus.Outstanding,
                Title = "",
                Type = ContractType.ItemExchange,
                Volume = 100
            };

            Assert.IsFalse(contractCheck.Contract(contract).IsValid);
        }

        [TestMethod]
        public void IsValidContract_PassInvalidShipFit_ReturnFalse()
        {
            Contract contract = new Contract()
            {
                AssigneeId = 234,
                Availability = ContractAvailability.Private,
                ContractId = 1,
                DateExpired = dateNow,
                DateIssued = dateNow,
                ForCorp = false,
                IssuerCorpId = 567,
                IssuerId = 890,
                Price = 123,
                ShipFitId = 9999,
                SolarSystemId = 123123,
                SolarSystemName = "Solar System Name",
                StartStationId = 321321,
                StartStationName = "Start Station Name",
                Status = ContractStatus.Outstanding,
                Title = "",
                Type = ContractType.ItemExchange,
                Volume = 100
            };

            Assert.IsFalse(contractCheck.Contract(contract).IsValid);
        }
    }
}
