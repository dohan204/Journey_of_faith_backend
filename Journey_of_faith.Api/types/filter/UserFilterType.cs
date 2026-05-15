using HotChocolate.Data.Filters;
using Journey_of_faith.Domain.entities;

namespace Journey_of_faith.Api.types.filter
{
    public class UserFilterType : FilterInputType<User>
    {
        protected override void Configure(IFilterInputTypeDescriptor<User> descriptor)
        {
            descriptor.BindFieldsExplicitly();

            descriptor.Field(f => f.Name).Type<CustomStringOperatioFilterInputType>();
            descriptor.Field(f => f.Email).Type<CustomStringOperatioFilterInputType>();
            base.Configure(descriptor);
        }
    }


    public class CustomStringOperatioFilterInputType : StringOperationFilterInputType
    {
        protected override void Configure(IFilterInputTypeDescriptor descriptor)
        
        {
            descriptor.Operation(DefaultFilterOperations.Equals).Type<StringType>();
            descriptor.Operation(DefaultFilterOperations.NotEquals).Type<StringType>();
            base.Configure(descriptor);
        }
    }
}
