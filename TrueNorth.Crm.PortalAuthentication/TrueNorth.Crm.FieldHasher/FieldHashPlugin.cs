using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TrueNorth.Crm.FieldHasher
{
    public class FieldHashPlugin : IPlugin
    {

        //Unsecure config should be in this format
        //   <Settings>
        //     <setting name = "PasswordField" >
        //         <value>tncore_password</value>
        //     </setting>
        //     <setting name = "DontHashField" >
        //         <value>tncore_donthash</value>
        //     </setting>
        //</Settings>

        private string passwordField;
        private string dontHashField;
        
        public FieldHashPlugin(string unsecureconfig, string securconfig)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(unsecureconfig);

            string passwordField = PluginConfiguration.GetConfigDataString(doc, "PasswordField");
            string dontHashField = PluginConfiguration.GetConfigDataString(doc, "DontHashField");

            this.passwordField = String.IsNullOrEmpty(passwordField) ? "tn_password" : passwordField;
            this.dontHashField = String.IsNullOrEmpty(dontHashField) ? "tn_donthash" : dontHashField;
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)
               serviceProvider.GetService(typeof(IPluginExecutionContext));

            var entity = (Entity)context.InputParameters["Target"];
            if (entity.Contains(passwordField))
            {
                if (entity.Attributes.Contains(dontHashField) && (bool)entity.Attributes[dontHashField])
                {
                    //dont hash it if don't hash is set to Yes
                    //this will be the case when the portal has already hashed a users password reset request for transmission
                }
                else
                {
                    //all other cases, hash the entered password
                    entity.Attributes[passwordField] = Hashor.HashPassword((string)entity.Attributes[passwordField]);
                }

                //always set dont hash to false afterwards
                entity.Attributes[dontHashField] = false;
            }
        }
    }
}
