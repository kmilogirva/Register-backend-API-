using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.DTOS
{
  public class ResponseMessages
  {
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; }

  }
  public class ResponseMessagesData<T> : ResponseMessages
  {
    public T Data { get; set; }
  }
}
