using System.ComponentModel.DataAnnotations;

namespace Framework.Tools;
public class Attribute
{
    public class TransactionTimeValidation : RangeAttribute
    {
        public TransactionTimeValidation()
          : base(typeof(DateTime), DateTime.Now.AddYears(-1).ToShortDateString(), 
                DateTime.Now.AddDays(1).ToShortDateString()) { }
    }
}