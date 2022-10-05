using OCRService.Domain;

namespace OCRService.Infrastructure
{
    public class MyOCRService : IOCRService
    {
        public string Get(int documentId)
        {
            Thread.Sleep(TimeSpan.FromSeconds(10));

            if (documentId > 10)
            {
                throw new ApplicationException("Przekroczono id");
            }

            return "Hello World!";
        }
    }
}