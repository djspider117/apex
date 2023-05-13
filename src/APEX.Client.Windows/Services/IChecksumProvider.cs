using System.Threading.Tasks;

namespace APEX.Client.Windows.Services
{
    public interface IChecksumProvider
    {
        string GetChecksum(string filePath);
    }
}