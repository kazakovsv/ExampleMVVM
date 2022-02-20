using System.ComponentModel;

namespace ExampleMVVM.DesktopClient.Models
{
    public enum CustomerType
    {
        [Description("(Not Specified)")]
        None,

        [Description("Person")]
        Person,

        [Description("Company")]
        Company
    }
}