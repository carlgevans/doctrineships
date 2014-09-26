namespace DoctrineShips.Test
{
    using DoctrineShips.Data;
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;
    using DoctrineShips.Service;
    using DoctrineShips.Service.Managers;
    using DoctrineShips.Validation;
    using DoctrineShips.Web.Controllers;
    using EveData;
    using GenericRepository;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Tools;

    [TestClass]
    public class ShipFitTests
    {
        private readonly Controller controller;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEveDataSource eveDataSource;
        private readonly IDoctrineShipsRepository doctrineShipsRepository;
        private readonly IDoctrineShipsValidation doctrineShipsValidation;
        private readonly IDoctrineShipsServices doctrineShipsServices;
        private readonly ISystemLogger logger;
        private readonly ISystemLoggerStore loggerStore;
        private readonly ShipFitManager shipFitManager;

        public ShipFitTests()
        {
            //this.doctrineShipsServices = new DoctrineShipsServices(doctrineShipsRepository, eveDataSource, doctrineShipsValidation, logger);
            this.controller = new SearchController(doctrineShipsServices);
            this.unitOfWork = new UnitOfWork(new DoctrineShipsContext());
            this.eveDataSource = new EveDataSourceCached(logger);
            this.doctrineShipsRepository = new DoctrineShipsRepository(unitOfWork);
            this.doctrineShipsValidation = new DoctrineShipsValidation(doctrineShipsRepository);
            this.shipFitManager = new ShipFitManager(doctrineShipsRepository, eveDataSource, doctrineShipsValidation, logger);
            this.loggerStore = new DoctrineShipsRepository(unitOfWork);
            this.logger = new SystemLogger(loggerStore);
        }

        ~ShipFitTests()
        {
            this.unitOfWork.Dispose();
        }

        [TestMethod]
        public void AddComponent_PassValidTypeName_MatchKnownValue()
        {
            Component component;
            string typeName = "Veldspar";
            int expectedTypeId = 1230;

            component = this.shipFitManager.AddComponent(typeName);

            if (component != null)
            {
                System.Diagnostics.Debug.WriteLine("Component Id: " + component.ComponentId);
                System.Diagnostics.Debug.WriteLine("Component Name: " + component.Name);
                System.Diagnostics.Debug.WriteLine("Component ImageUrl: " + component.ImageUrl);
                System.Diagnostics.Debug.WriteLine("Component Volume: " + component.Volume);

                Assert.AreEqual(expectedTypeId, component.ComponentId);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AddComponent_PassValidRubbish_ReturnsNull()
        {
            Component component;
            string typeName = "RubbishRubbishRubbish";

            component = this.shipFitManager.AddComponent(typeName);

            Assert.IsNull(component);
        }

        [TestMethod]
        public void ExportShipFit()
        {
            List<Doctrine> doctrines = (List<Doctrine>)doctrineShipsRepository.GetDoctrines();
            if (doctrines.Count == 0) throw new Exception("No Doctrines Available.");
            if (doctrines[0].DoctrineShipFits.Count == 0) throw new Exception("No ship fits available in the first doctrine.");
            List<DoctrineShipFit> fits = (List<DoctrineShipFit>)doctrines[0].DoctrineShipFits;
            Debug.WriteLine(fits[0].ShipFit.ToXML());
        }
    }
}
