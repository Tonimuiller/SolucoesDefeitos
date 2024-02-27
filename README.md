**

## Solução e Defeito
**

**Sistema para criar base de conhecimento de soluções e defeitos de equipamentos para estabelecimentos de manutenção em geral.**

Sistema para registro de defeitos e soluções para problemas e manutenções em geral. Empresas de manutenção automotiva, telefonia, equipamentos eletrônicos, equipamentos de informática e outros setores podem utilizar esse sistema para registrar defeitos/problemas e soluções em equipamentos. É possível pesquisar e filtrar por fabricante, grupo e modelo do equipamento. Além de permitir cadastrar uma descrição para o defeito e um texto explicativo para a solução, também permite anexar fotos e links de vídeos do Youtube (com player embutido) para enriquecer mais ainda com detalhes o registro de soluções e defeitos.

O sistema foi desenvolvido como base de pesquisa e exploração de tecnologias do meu interesse e para uso em uma oficina de carros da minha família. O stack utilizado no desenvolvimento do sistema foi:

 - Net Core 7
 - C#
 - Dapper
 - Onion Architecture
 - Razor Pages
 - Minimal Apis
 - JQuery
 - Bootstrap 5
 - MySql

**Justificando algumas escolhas:**

 - **Dapper** - Escolhi o micro ORM Dapper porque queria ter a liberdade de montar minhas próprias instruções SQL e para praticar escreve-las visto que acho valioso em algumas circunstâncias profissionais saber bem o SQL. No caso desse projeto poderia perfeitamente ter escolhido outro framework ORM como o Entity Framework Core ou o NHibernate mas à título de treinamento e prazer em escrever comandos SQL escolhi o Dapper;
 - **Razor Pages** - Escolhi o Razor pages porque tinha curiosidade de saber como funcionava a ferramenta e provar se a forma de trabalhar com modelos de páginas bastante semelhante ao padrão MVP(Model View Presenter) seria interessante. Gostei bastante e acho que é uma boa opção ao MVC "tradicional". Poderia ter feito com algum framework SPA também. Já trabalhei com Angular e VueJs e gostei de ambos, apesar de me identificar mais com o Angular.

**Rodando o projeto**

1. Crie um banco MySql no seu computador local ou no servidor que achar apropriado;
2. Execute o script encontrado em Solucoes.Defeitos.DataAccess/Migrations/dump-solucaodefeito-202306291224.sql no banco criado;
3. Ajuste a connection string com o banco contida em SolucoesDefaitos.Presentation/SolucoesDefaitos.Presentation.RazorPages/appsettings.Development.json
4. Execute o sistema utilizando preferencialmente o Visual Studio, Jetbrains Rider ou outra IDE/editor de sua preferência.
5. Ao executar o sistema, se a tabela de usuários estiver vazia, será criado um usuário com o login "admin" e senha "12345" automaticamente. A gestão de usuários está em fase de desenvolvimento.
