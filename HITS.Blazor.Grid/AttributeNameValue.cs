namespace HITS.Blazor.Grid
{
    public class AttributeNameValue
    {
        public string AttributeName { get; set; }
        public object AttributeValue { get; set; }

        public AttributeNameValue()
        {

        }

        public AttributeNameValue(string attributeName, object attributeValue)
        {
            AttributeName = attributeName;
            AttributeValue = attributeValue;
        }

    }
}
