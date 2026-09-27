namespace PatinhasApp.Models;

// Listas fixas de opções. Guardamos o número (0,1,2...) no banco,
// mas mostramos o texto amigável nas telas.

public enum Sexo
{
    Macho = 0,
    Femea = 1
}

public enum Porte
{
    Pequeno = 0,
    Medio = 1,
    Grande = 2
}

public enum SituacaoCao
{
    NoProjeto = 0, // cão do projeto
    Apoiado = 1,   // cão do amigo que o projeto ajuda
    Adotado = 2
}

public enum TipoRegistroSaude
{
    Vacina = 0,
    Vermifugo = 1,
    Problema = 2 // problema de saúde
}
