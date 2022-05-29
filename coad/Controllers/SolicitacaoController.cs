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
