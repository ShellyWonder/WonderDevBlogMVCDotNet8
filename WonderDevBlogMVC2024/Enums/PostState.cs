using System.ComponentModel;

namespace WonderDevBlogMVC2024.Enums
{
    public enum PostState
    {
        [Description("Production Ready")]
        ProductionReady,


        [Description("In Process")]
        InDevelopment,

        [Description("Awaiting Preview")]
        PreviewReady
    }
}

