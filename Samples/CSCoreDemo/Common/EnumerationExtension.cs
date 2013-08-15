using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;

namespace CSCoreDemo.Common
{
    public class EnumerationExtension : MarkupExtension
    {
        private Type _enumType;

        public EnumerationExtension(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            EnumType = enumType;
        }

        public Type EnumType
        {
            get
            {
                return _enumType;
            }
            set
            {
                if (_enumType == value)
                    return;

                if ((Nullable.GetUnderlyingType(value) ?? value).IsEnum == false)
                    throw new ArgumentException("Type is no enumtype.", "value");

                _enumType = value;
            }
        }

        public string GetDescription(object enumValue)
        {
            if (enumValue == null)
                throw new ArgumentNullException("enumValue");

            var descAttribute = EnumType.GetField(enumValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return descAttribute != null ? descAttribute.Description : enumValue.ToString();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var values = Enum.GetValues(EnumType);
            return (from object v in values select new EnumerationMember() { Value = v, Description = GetDescription(v) }).ToArray();
        }
    }

    public class EnumerationMember
    {
        public string Description { get; set; }

        public object Value { get; set; }
    }
}