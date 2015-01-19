namespace DoctrineShips.Repository.Operations
{
    using System.Collections.Generic;
    using System.Linq;
    using DoctrineShips.Entities;
    using GenericRepository;
    using System.Data.SqlClient;
    using System;
    using System.Reflection;

    internal sealed class ComponentOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal ComponentOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteComponent(int componentId)
        {
            this.unitOfWork.Repository<Component>().Delete(componentId);
        }

        internal void UpdateComponent(Component component)
        {
            component.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<Component>().Update(component);
        }

        internal Component AddComponent(Component component)
        {
            this.unitOfWork.Repository<Component>().Insert(component);
            return component;
        }

        internal Component CreateComponent(Component component)
        {
            component.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<Component>().Insert(component);
            return component;
        }

        internal Component GetComponent(int componentId)
        {
            return this.unitOfWork.Repository<Component>().Find(componentId);
        }

        internal Component GetComponent(string componentName)
        {
            int id = 0;
            SqlConnection conn = new SqlConnection(DoctrineShipsRepository.ConnectionString);
            SqlCommand sql = new SqlCommand("select ComponentId from dbo.Components where lower(name) = lower(@componentName)", conn);
            sql.Parameters.Add(new SqlParameter("componentName", componentName));
            try
            {
                conn.Open();
                id = (int)sql.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //logger.LogMessage("An error occured while loading the CCP CorporationContractItems Xml for ApiID: " + apiId + ". ContractId: " + contractId + ". Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                //logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);

                //fail silently because jesus christ this project.
            }
            finally { conn.Close(); }
            return GetComponent(id);
        }

        internal IEnumerable<Component> GetComponents()
        {
            var components = this.unitOfWork.Repository<Component>()
                              .Query()
                              .Get()
                              .OrderBy(x => x.Name)
                              .ToList();

            return components;
        }
    }
}