# 🐾 Patinhas do Bem

Aplicativo **Android** para um projeto de resgate de cães. Serve para registrar
os cães, o histórico de saúde (vacinas, vermífugos e problemas) e as adoções —
tudo em um só lugar, **offline**, sem login e sem nuvem.

Este repositório é o **esqueleto (estrutura base) do projeto**, já modelado e com
o **banco de dados pronto**, para o grupo continuar o desenvolvimento por cima.

---

## 1. O que o app já faz

- **Cadastro de cães**: nome, foto, sexo, porte, data do resgate, situação
  (no projeto / apoiado / adotado) e observações.
- **Histórico de saúde** por cão: vacinas, vermífugos e problemas de saúde, cada
  um com data e **próxima data prevista**.
- **Alertas**: lista automática do que está vencido ou vence nos **próximos 30 dias**.
- **Controle de adoções**: cão, data, nome do adotante e contato — guardados juntos.
  Ao registrar uma adoção, o cão vira automaticamente **"Adotado"**.
- **Backup**: botão para **exportar** todos os dados (um arquivo `.db3`) e
  **restaurar** depois — útil ao trocar de celular.

Tudo funciona **100% no aparelho**. O app **não usa internet**.

---

## 2. Por que .NET MAUI + C#

A cliente pediu **app Android** e **C# como linguagem principal**. O
**.NET MAUI** é a tecnologia da Microsoft para criar apps nativos usando C#,
então atende aos dois pedidos com um único projeto. Como o uso é só no Android,
o projeto está configurado **apenas para Android** (`net8.0-android`), o que deixa
tudo mais simples de compilar e manter.

Escolhas técnicas e o porquê:

| Escolha | Motivo |
|--------|--------|
| **.NET 8 (LTS)** | Versão de suporte estendido, estável para um projeto que vai durar. |
| **SQLite** (`sqlite-net-pcl`) | Banco local em arquivo único, perfeito para 1 usuária, offline. |
| **MVVM** (`CommunityToolkit.Mvvm`) | Separa tela (XAML) da lógica (ViewModel); código organizado e fácil de crescer. |
| **Shell + abas** | Navegação simples entre as 4 áreas do app. |
| **Sem login / sem nuvem** | Combinado com a cliente: 1 pessoa, 1 celular. Menos complexidade. |

---

## 3. Como abrir e rodar

### Requisitos
- **Visual Studio 2022** (Windows) **ou** VS Code, com a carga de trabalho
  **".NET Multi-platform App UI development"** instalada (ela traz o SDK do
  Android e o workload do MAUI).
- .NET 8 SDK.

### Passos
1. Abra o arquivo **`PatinhasApp.sln`** no Visual Studio.
2. Aguarde o VS restaurar os pacotes NuGet (baixa sozinho na primeira vez).
3. Selecione um **emulador Android** ou um **celular conectado** (com depuração
   USB ligada) como destino.
4. Clique em **Executar (▶)**.

> ⚠️ **Observação:** este esqueleto foi entregue como **código-fonte**. Ele
> **precisa ser compilado no Visual Studio** com o SDK do Android — não é possível
> gerar o APK sem esse ambiente. Nada aqui foi pré-compilado.

---

## 4. Como gerar o APK (sem Play Store)

Como a instalação será direta pelo APK:

1. No Visual Studio, troque a configuração de **Debug** para **Release**.
2. Menu **Build → Publish** (ou clique com o botão direito no projeto →
   **Publish / Archive**).
3. O VS gera um arquivo **`.apk`** (ou `.aab`). Para instalação manual, use o
   **`.apk`**.
4. Envie o `.apk` para o celular (WhatsApp, cabo, Drive…) e instale. Talvez seja
   preciso permitir **"instalar de fontes desconhecidas"** nas configurações do
   Android.

> Para um app "de verdade" (que atualiza sem reinstalar do zero) é recomendável
> **assinar o APK** com uma *keystore*. Isso pode ficar para uma etapa futura.

---

## 5. Estrutura das pastas

```
PatinhasApp/
├── PatinhasApp.sln              → Solução (abre isto no Visual Studio)
├── README.md                    → Este arquivo
├── .gitignore
└── PatinhasApp/PatinhasApp/     → O projeto em si
    ├── PatinhasApp.csproj       → Configuração do projeto e pacotes
    ├── MauiProgram.cs           → "Liga" tudo (banco, serviços, telas)
    ├── App.xaml(.cs)            → Início do app + carrega estilos
    ├── AppShell.xaml(.cs)       → Abas e rotas de navegação
    │
    ├── Models/                  → As "coisas" do app (viram tabelas no banco)
    │   ├── Enums.cs             → Listas fixas (Sexo, Porte, Situação, Tipo)
    │   ├── Cao.cs               → Um cão
    │   ├── RegistroSaude.cs     → Uma vacina/vermífugo/problema
    │   └── Adocao.cs            → Uma adoção
    │
    ├── Data/
    │   └── DatabaseService.cs   → TODO o acesso ao banco SQLite (CRUD)
    │
    ├── Services/
    │   └── BackupService.cs     → Exportar e restaurar o banco
    │
    ├── Helpers/
    │   └── Converters.cs        → Ajustes de exibição (ex.: foto padrão)
    │
    ├── ViewModels/              → A lógica de cada tela (o "cérebro")
    │   ├── BaseViewModel.cs
    │   ├── CaesViewModel.cs / CadastroCaoViewModel.cs / DetalheCaoViewModel.cs
    │   ├── CadastroSaudeViewModel.cs
    │   ├── AdocoesViewModel.cs / CadastroAdocaoViewModel.cs
    │   ├── AlertasViewModel.cs
    │   └── AjustesViewModel.cs
    │
    ├── Views/                   → As telas (XAML = aparência + .cs = ligação)
    │   ├── CaesPage / CadastroCaoPage / DetalheCaoPage
    │   ├── CadastroSaudePage
    │   ├── AdocoesPage / CadastroAdocaoPage
    │   ├── AlertasPage
    │   └── AjustesPage
    │
    ├── Resources/
    │   ├── Styles/Colors.xaml   → Paleta de cores
    │   ├── Styles/Styles.xaml   → Estilos (cartões, botões, etc.)
    │   ├── Images/dog_placeholder.png → Foto padrão quando não há foto
    │   ├── AppIcon/             → Ícone do app (patinha)
    │   └── Splash/              → Tela de abertura
    │
    └── Platforms/Android/       → Arquivos específicos do Android
        ├── MainActivity.cs
        ├── MainApplication.cs
        └── AndroidManifest.xml  → Permissões (câmera; SEM internet)
```

---

## 6. Como o código está organizado (MVVM)

O app segue o padrão **MVVM (Model-View-ViewModel)**, que separa em 3 camadas:

- **Model** (`Models/`): representa os dados. Cada classe (`Cao`, `RegistroSaude`,
  `Adocao`) vira uma **tabela** no SQLite.
- **View** (`Views/`): a tela em si, escrita em **XAML**. Ela **não tem lógica** —
  só mostra dados e dispara comandos.
- **ViewModel** (`ViewModels/`): o "cérebro" da tela. Guarda os dados que a tela
  mostra e os **comandos** (o que acontece ao tocar num botão).

**Fluxo de um exemplo (abrir a lista de cães):**

1. `CaesPage` aparece na tela e, no `OnAppearing`, chama `CarregarAsync()` do
   `CaesViewModel`.
2. O ViewModel pede a lista ao `DatabaseService`.
3. O `DatabaseService` lê do SQLite e devolve os cães.
4. O ViewModel coloca os cães numa lista observável (`Caes`).
5. A tela, que está "amarrada" (binding) nessa lista, **se atualiza sozinha**.

Isso deixa o código fácil de testar e de dividir entre o grupo: uma pessoa mexe
na aparência (XAML), outra na lógica (ViewModel), sem pisar no pé uma da outra.

---

## 7. O banco de dados (já pronto)

Todo o acesso ao banco fica em **`Data/DatabaseService.cs`**. As telas nunca
falam com o SQLite diretamente — sempre passam por esse serviço.

- O banco é **um arquivo** chamado **`patinhas.db3`**, guardado numa pasta
  **privada do app** dentro do celular.
- As tabelas são criadas **automaticamente** na primeira vez que o app roda
  (método `InicializarAsync`), a partir das classes de `Models/`.
- Há **3 tabelas**: `caes`, `registros_saude` e `adocoes`.
- Os registros de saúde e as adoções guardam o **`CaoId`** para saber a qual cão
  pertencem. Ao **excluir um cão**, o histórico e a adoção dele também são
  removidos (feito no `ExcluirCaoAsync`).

O serviço já traz os métodos prontos para **criar, ler, atualizar e excluir**
(CRUD) cães, registros de saúde e adoções.

---

## 8. Como funciona o backup

Em **`Services/BackupService.cs`**:

- **Exportar**: como o banco é um único arquivo, o app faz uma **cópia** dele e
  abre a tela de **compartilhamento** do Android. A cliente pode salvar no Google
  Drive, mandar por e-mail/WhatsApp, etc.
- **Restaurar**: o app deixa escolher um arquivo `.db3` de backup e **substitui**
  o banco atual por ele (com uma confirmação antes, porque isso apaga os dados
  atuais).

Assim, ao trocar de celular, basta exportar no aparelho antigo e restaurar no novo.

---

## 9. Cores e visual

A identidade visual está centralizada em **`Resources/Styles/`**:

- **Colors.xaml**: a paleta. Cor principal **coral `#E07856`**, apoio em
  **verde `#3D8577`**, fundo off-white quente `#FBF7F2`. Para mudar a "cara" do
  app, basta trocar valores aqui.
- **Styles.xaml**: estilos reutilizados (cartões arredondados, botões, títulos,
  botão flutuante "+").

O tom foi pensado para algo **acolhedor e simples**, combinando com um projeto de
cuidado com animais, e priorizando **facilidade de uso** (poucos toques, telas
limpas), já que sobra pouco tempo no dia a dia.

---

## 10. Próximos passos sugeridos para o grupo

Ideias para evoluir a partir daqui (fora do escopo atual, mas fáceis de encaixar):

- **Notificações locais** para os alertas de vacina/vermífugo.
- **Busca por nome** na lista de cães.
- **Backup automático** agendado.
- **Editar/excluir adoção** (hoje há cadastro e listagem).
- **Campos extras** no cão (raça, cor, castrado sim/não).

Como a arquitetura é organizada (MVVM + serviço de banco), cada uma dessas
melhorias mexe em poucos arquivos.

---

Feito com 🐾 para ajudar quem ajuda os cães.
