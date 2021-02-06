using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
//using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            IOrganizationService organization = ConnectD35OnlineUsingOrgSvc();

            QueryExpression queryExpression = new QueryExpression("account");
            queryExpression.ColumnSet.AllColumns = true;
            queryExpression.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, "Inactive"));
            EntityCollection config = organization.RetrieveMultiple(queryExpression);
            int count = config.Entities.Count;

          //  Entity newconfig = organization.RetrieveMultiple(queryExpression).Entities.FirstOrDefault();

            for (int i = 0; i < count; i++)
            {
                //  foreach (Entity record in config.Entities)
                // {
                //  string val = record.Attributes["name"].ToString();

                Entity newconfig = organization.RetrieveMultiple(queryExpression).Entities.FirstOrDefault();
                organization.Delete(newconfig.LogicalName, newconfig.Id);
            }
           // }
        }

        public static IOrganizationService ConnectD35OnlineUsingOrgSvc()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            IOrganizationService organizationService = null;
            String username = "imad@imadimadtrial.onmicrosoft.com";
            String password = "trident123@";
            String url = "https://imadimadtrial.api.crm8.dynamics.com/XRMServices/2011/Organization.svc";
            try
            {
                ClientCredentials clientCredentials = new ClientCredentials();
                clientCredentials.UserName.UserName = username;
                clientCredentials.UserName.Password = password;

                // For Dynamics 365 Customer Engagement V9.X, set Security Protocol as TLS12
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                organizationService = (IOrganizationService)new OrganizationServiceProxy(new Uri(url), null, clientCredentials, null);

                if (organizationService != null)
                {
                    Guid gOrgId = ((WhoAmIResponse)organizationService.Execute(new WhoAmIRequest())).OrganizationId;
                    if (gOrgId != Guid.Empty)
                    {
                        Console.WriteLine("Connection Established Successfully...");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to Established Connection!!!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured - " + ex.Message);
            }
            return organizationService;

        }
    }
}
