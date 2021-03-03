namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Helpers
{
    public static class Constants
    {
        public static class ErrorMessges
        {
            public const string UnableToCreateTable = "Unable to find/create the table name specified.Because of the exception {0} from Azure.";
            public const string TableOperationFailure = "Unable to complete the requested table operation.Because of the exception {0} from Azure." +
                "Check out https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model for entity model structure.";
            public const string EntityIdNotFound = "Unable to complete the requested table operation.Because of the exception Requested id not found from Azure.";
            public const string NullReferenceForId = "Id can't be null for the entity";
        }
    }
}
