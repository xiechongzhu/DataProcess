using System;
using DevExpress.Xpo;

namespace DataProcess.CustomControl
{

    public class ProgressbarToNumber : XPObject
    {
        public ProgressbarToNumber() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ProgressbarToNumber(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
    }

}