using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System.Text;

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


    public enum CcmBtnType
    {
        Action,
        Submit,
        Cancel,
        Delete
    }

    public enum CcmBtnSize
    {
        Normal
    }

    public enum CcmIco
    {
        Submit,
        Cancel,
        Delete,
        Plus,
        Minus
    }

    public abstract class CoreCcmBtnTagHelper :
        TagHelper
    {
        public CcmBtnType Type { get; set; }
        public CcmBtnSize Size { get; set; }
        public bool RenderCode { get; set; }
        public bool Disabled { get; set; }

        protected virtual StringBuilder GetClass()
        {
            var @class = new StringBuilder("ccm-btn");

            if (this.Size == CcmBtnSize.Normal)
                @class.Append(" normal");

            switch (this.Type)
            {
                case CcmBtnType.Action:
                    @class.Append(" action-btn");
                    break;
                case CcmBtnType.Cancel:
                    @class.Append(" cancel-btn");
                    break;
                case CcmBtnType.Delete:
                    @class.Append(" delete-btn");
                    break;
                case CcmBtnType.Submit:
                    @class.Append(" submit-btn");
                    break;

                default:
                    break;
            }

            return @class;
        }
    }

    [HtmlTargetElement("ccm-btn-ico", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class CcmBtnIcoTagHelper :
        CoreCcmBtnTagHelper
    {
        public CcmIco Ico { get; set; }

        protected override StringBuilder GetClass()
        {
            var @class = base.GetClass();
            @class.Append(" ccm-ico");

            switch (this.Ico)
            {
                case CcmIco.Submit:
                    @class.Append(" ico-submit");
                    break;
                case CcmIco.Cancel:
                    @class.Append(" ico-cancel");
                    break;
                case CcmIco.Delete:
                    @class.Append(" ico-delete");
                    break;
                case CcmIco.Plus:
                    @class.Append(" ico-plus");
                    break;
                case CcmIco.Minus:
                    @class.Append(" ico-minus");
                    break;
                default:
                    break;
            }

            return @class;
        }

        public override void Process(
            TagHelperContext context,
            TagHelperOutput output)
        {
            var @class = this.GetClass()
                .ToString();

            var disabled = this.Disabled ? "disabled" : "";

            if (this.RenderCode)
            {
                output.TagName = "code";
                output.Content.SetContent($"<button class='{@class}' {disabled}></button>");
                output.TagMode = TagMode.StartTagAndEndTag;
            }
            else
            {
                output.TagName = "button";
                output.Attributes.Add("class", @class);
                if (Disabled)
                    output.Attributes.Add("disabled", "disabled");
                output.TagMode = TagMode.StartTagAndEndTag;
            }
        }
    }

    [HtmlTargetElement("ccm-btn", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class CcmBtnTagHelper :
        CoreCcmBtnTagHelper
    {
        public string Text { get; set; }

        public override void Process(
            TagHelperContext context,
            TagHelperOutput output)
        {
            var @class = this.GetClass()
                .ToString();

            var disabled = this.Disabled ? "disabled" : "";


            if (this.RenderCode)
            {
                output.TagName = "code";
                output.Content.SetContent($"<button class='{@class}' {disabled}>{Text}</button>");
                output.TagMode = TagMode.StartTagAndEndTag;
            }
            else
            {
                output.TagName = "button";
                output.Attributes.Add("class", @class);
                if (Disabled)
                    output.Attributes.Add("disabled", "disabled");
                output.Content.SetContent(this.Text);
                output.TagMode = TagMode.StartTagAndEndTag;
            }
        }
    }
}
