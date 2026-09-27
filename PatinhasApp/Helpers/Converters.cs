using System.Globalization;

namespace PatinhasApp.Helpers;

// Converte o caminho da foto (texto) em uma imagem para a tela.
// Se não houver foto ou o arquivo não existir, mostra a imagem padrão.
public class CaminhoFotoConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var caminho = value as string;

        if (!string.IsNullOrWhiteSpace(caminho) && File.Exists(caminho))
            return ImageSource.FromFile(caminho);

        return ImageSource.FromFile("dog_placeholder.png");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

// Retorna true quando o texto NÃO está vazio. Útil para mostrar/esconder
// um bloco na tela só quando há conteúdo (ex.: observações).
public class StringNaoVazioConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => !string.IsNullOrWhiteSpace(value as string);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

// Inverte um booleano (true -> false). Útil para "esconder quando ocupado", etc.
public class BoolInvertidoConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b ? !b : value!;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b ? !b : value!;
}
