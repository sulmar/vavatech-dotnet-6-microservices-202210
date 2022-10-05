using System.Reflection.Metadata;

namespace OCRService.Domain
{
    public interface IOCRService
    {
        string Get(int documentId);
    }
}