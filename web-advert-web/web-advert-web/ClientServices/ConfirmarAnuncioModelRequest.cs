using System;
namespace web_advert_web.ClientServices
{
    public class ConfirmarAnuncioModelRequest
    {
        public ConfirmarAnuncioModelRequest()
        {
        }

        public string Id { get; set; }
        public StatusAnuncio Status { get; set; }
    }
}
