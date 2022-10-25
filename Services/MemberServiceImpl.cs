using System.Security.Cryptography;
using System.Text;

public class MemberServiceImpl : MemberService
{
    private readonly MollaContext context;
    public MemberServiceImpl(MollaContext context)
    {
        this.context = context;
    }
    public Member Login(string loginname, string password)
    {
        string LoginName = loginname, Password = GetMD5Hash(password);
        return context.members.SingleOrDefault(a => a.LoginName == LoginName && a.Password == Password);
    }
    public string GetMD5Hash(string input)
    {
        using (MD5 md5Hash = MD5.Create())
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}