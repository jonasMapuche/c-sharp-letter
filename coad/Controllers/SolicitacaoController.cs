using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace coad.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolicitacaoController : ControllerBase
    {
        
        [HttpGet("ListaSolicitacao")]
        public IEnumerable<Solicitacao> GetListaSolicitacao()
        {
            List<Solicitacao> solicitacao = new List<Solicitacao>();

            solicitacao = Deserializar();

            return solicitacao.Select(index => new Solicitacao
            {
                Id = index.Id,
                Titulo = index.Titulo,
                Tipo = index.Tipo,
                Descricao = index.Descricao,
                Data = index.Data,
                Nome = index.Nome,
                Telefone = index.Telefone,
                Email = index.Email,
                Status = index.Status
            })
            .ToArray();

        }

        [HttpGet("IdSolicitacao")]
        public IEnumerable<Solicitacao> GetListaSolicitacao(int id)
        {
            List<Solicitacao> solicitacao = new List<Solicitacao>();

            solicitacao = Deserializar();

            return solicitacao.FindAll(index => index.Id == id);

        }

        [HttpGet("TituloSolicitacao")]
        public IEnumerable<Solicitacao> GetTituloSolicitacao(string titulo)
        {
            List<Solicitacao> solicitacao = new List<Solicitacao>();

            solicitacao = Deserializar();

            return solicitacao.FindAll(index => index.Titulo == titulo);

        }

        [HttpGet("NomeSolicitacao")]
        public IEnumerable<Solicitacao> GetNomeSolicitacao(string nome)
        {
            List<Solicitacao> solicitacao = new List<Solicitacao>();

            solicitacao = Deserializar();

            return solicitacao.FindAll(index => index.Nome == nome);

        }

        [HttpGet("EmailSolicitacao")]
        public IEnumerable<Solicitacao> EmailSolicitacao(string email)
        {
            List<Solicitacao> solicitacao = new List<Solicitacao>();

            solicitacao = Deserializar();

            return solicitacao.FindAll(index => index.Email == email);

        }

        [HttpGet("PaginaSolicitacao")]
        public IEnumerable<Solicitacao> PaginaSolicitacao(string tamanho, string pagina)
        {
            int resto = 0;
            int inicio, fim = 0;
            decimal divisaoDecimal = 0;
            int divisao = 0;
            int parteInteira = 0;
            List<Solicitacao> solicitacao = new List<Solicitacao>();
            //Erro erro = new Erro();

            solicitacao = Deserializar();

            resto = solicitacao.Count() % Int32.Parse(tamanho);
            divisaoDecimal = solicitacao.Count() / Int32.Parse(tamanho);
            divisao = Int32.Parse(divisaoDecimal.ToString());
            parteInteira = Int32.Parse(Math.Truncate(divisaoDecimal).ToString());

            if (Int32.Parse(pagina) >= divisao) {
                if (resto == 0 && Int32.Parse(pagina) <= parteInteira) {
                    inicio = ((Int32.Parse(tamanho) * Int32.Parse(pagina)) - Int32.Parse(pagina));
                    fim = ((Int32.Parse(tamanho) * Int32.Parse(pagina)) - Int32.Parse(pagina)) + Int32.Parse(tamanho) - 1;
                    return solicitacao.GetRange(inicio, fim).ToArray();
                } else {
                    inicio = ((Int32.Parse(tamanho) * Int32.Parse(pagina)) - Int32.Parse(pagina));
                    fim = solicitacao.Count();
                    return solicitacao.GetRange(inicio, fim).ToArray();
                }
            } else {
                return null;
            }
        }

        [HttpGet("PrioridadeSolicitacao")]
        public IEnumerable<Solicitacao> PrioridadeSolicitacao(string status, string data)
        {
            Int64 dataNumero = Int64.Parse(data);
            string dataConvertida = data.Substring(7, 2) + "/" + data.Substring(5, 2) + "/" + data.Substring(1, 4);
            DateTime dataInicio = DateTime.Now;
            DateTime dataFim = DateTime.Now;
            List <Solicitacao> solicitacao = new List<Solicitacao>();
            dataNumero = Int64.Parse(data);
            solicitacao = Deserializar();
            dataInicio = Convert.ToDateTime(dataConvertida).AddDays(-2);
            dataFim = Convert.ToDateTime(dataConvertida).AddDays(-1);

            if (status == "Baixa") {
                return solicitacao.FindAll(index => Int64.Parse(index.Data) == dataNumero);
            } else {
                if (status == "Media") {
                    return solicitacao.FindAll(index => (Convert.ToDateTime(index.Data.Substring(7, 2) + "/" + index.Data.Substring(5, 2) + "/" + index.Data.Substring(1, 4)) >= dataInicio) && (Convert.ToDateTime(index.Data.Substring(7, 2) + "/" + index.Data.Substring(5, 2) + "/" + index.Data.Substring(1, 4)) <= dataFim));
                } else
                {
                    return solicitacao.FindAll(index => (Convert.ToDateTime(index.Data.Substring(7, 2) + "/" + index.Data.Substring(5, 2) + "/" + index.Data.Substring(1, 4)) < dataInicio));
                }
            }
        }

        [HttpPost("GravarSolicitacao")]
        public String PostGravarSolicitacao([FromBody]Solicitacao objeto)
        {
            List<Solicitacao> solicitacao = new List<Solicitacao>();

            solicitacao = Deserializar();

            solicitacao.Add(objeto);

            Serializar(solicitacao);

            return "{Gravado}";

        }

        [HttpDelete("DeleteSolicitacao")]
        public String DeleteSolicitacao(int id)
        {
            List<Solicitacao> solicitacao = new List<Solicitacao>();

            solicitacao = Deserializar();

            //Solicitacao objeto = new Solicitacao();

            solicitacao.Remove(solicitacao.FirstOrDefault(index => index.Id == id));

            Serializar(solicitacao);

            return "{Removido}";

        }

        private static void Serializar(List<Solicitacao> objeto)
        {
            using (var stream = new System.IO.FileStream("solicitacao.json", System.IO.FileMode.Create))
            {
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<Solicitacao>));
                serializer.WriteObject(stream, objeto);
            }
        }

        private static List<Solicitacao> Deserializar()
        {
            using (var stream = new System.IO.FileStream("solicitacao.json", System.IO.FileMode.Open))
            {
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<Solicitacao>));
                return (List<Solicitacao>)serializer.ReadObject(stream);
            }
        }
    }
}
