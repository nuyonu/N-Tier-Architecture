namespace N_Tier.Application.Common.Email;

public class EmailAttachment
{
    public byte[] Value { get; private set; }

    public string Name { get; private set; }

    public static EmailAttachment Create(byte[] value, string name)
    {
        return new EmailAttachment
        {
            Value = value,
            Name = name
        };
    }
}
