using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using satellite.Controller;

namespace satellite.Model
{
    public class Model
    {
        Controller.Controller controller { set; get; } = null;
        public Model(Controller.Controller controller)
        {
            this.controller = controller;
        }
        //classes +++ 





    }
}
