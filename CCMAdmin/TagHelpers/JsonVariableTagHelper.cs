using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;

namespace CCMAdmin.TagHelpers
{
    [HtmlTargetElement("json-variable", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class JsonVariableTagHelper :
        TagHelper
    {
        #region PUBLIC PROPERTIES
        public string Name { get; set; }
        public object Data { get; set; }
        #endregion

        #region PUBLIC METHODS
        public override void Process(
            TagHelperContext context,
            TagHelperOutput output)
        {
            var json = JsonConvert
                .SerializeObject(this.Data);

            output.TagName = "script";
            output.Attributes.Add("type", "text/javascript");
            output.Content.SetHtmlContent($"let {this.Name} = {json};");
            output.TagMode = TagMode.StartTagAndEndTag;
        }
        #endregion
    }

    [HtmlTargetElement("button-code", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ButtonCodeTagHelper:
        TagHelper
    {

        public string Class { get; set; }
        public string Text { get; set; }

        public override void Process(
            TagHelperContext context,
            TagHelperOutput output)
        {
            //var json = JsonConvert
            //    .SerializeObject(this.Data);

            output.TagName = "code";
            output.Content.SetContent($"<button class='{Class}'>{Text}</button>");
            //output.Attributes.Add("type", "text/javascript");
            //output.Content.SetHtmlContent($"let {this.Name} = {json};");
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
