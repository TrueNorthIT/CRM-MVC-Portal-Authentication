using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueNorth.Crm.FieldHasher
{
    public class FieldHashPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)
               serviceProvider.GetService(typeof(IPluginExecutionContext));

            var entity = (Entity)context.InputParameters["Target"];
            if (entity.Contains("tn_password"))
            {
                if (entity.Attributes.Contains("tn_donthash") && (bool)entity.Attributes["tn_donthash"])
                {
                    //dont hash it if don't hash is set to Yes
                    //this will be the case when the portal has already hashed a users password reset request for transmission
                }
                else
                {
                    //all other cases, hash the entered password
                    entity.Attributes["tn_password"] = Hashor.HashPassword((string)entity.Attributes["tn_password"]);
                }

                //always set dont hash to false afterwards
                entity.Attributes["tn_donthash"] = false;
            }
        }
    }
}
