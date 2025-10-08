﻿
using Jogos.Domain.Enuns;
using System.ComponentModel.DataAnnotations;

namespace Jogos.Application.DTOs
{
    public class AtualizarJogoDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        [MinLength(2, ErrorMessage = "O título deve ter no mínimo 2 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public CategoriaJogo Categoria { get; set; }

        [Required]
        [Range(0.01, 10000, ErrorMessage = "O preço deve ser entre R$ 0,01 e R$ 10.000,00.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A data de lançamento é obrigatória.")]
        public DateTime DataLancamento { get; set; }
    }

}
