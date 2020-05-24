using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using Androtomist.Models.Database.Inputs;
using System.Reflection;
using Androtomist.Models.Database.Entities;

namespace Androtomist.Models.Database
{
    public class FormElement<TModel> : RazorPage<TModel>
    {
        public override Task ExecuteAsync()
        {

            throw new NotImplementedException();
        }

        public void F(INPUT Input, string extraFilter = "")
        {
            InputAbstract inputAbstract;
            //Model;

            
            PropertyInfo[] props = Model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if ((bool)props.FirstOrDefault(x => x.Name.Equals("ExistsProp")).GetValue(Model))
                extraFilter = props.FirstOrDefault(x => x.Name.Equals(Input.ToString())).GetValue(Model).ToString();

			switch (Input)
            {
				//file
				case INPUT.SUB_ID: inputAbstract = new SubID(extraFilter); break;
                //process
                case INPUT.P_NAME: inputAbstract = new PName(extraFilter); break;
                case INPUT.P_TYPE_ID: inputAbstract = new PTypeID(extraFilter); break;
                case INPUT.P_FILE_ID: inputAbstract = new FileID(extraFilter); break;
                case INPUT.FILE_LABEL: inputAbstract = new FileLabel(extraFilter); break;

                default: throw new Exception("Wrong input type.");
            }

            WriteLiteral(inputAbstract.html);
        }
    }
}
