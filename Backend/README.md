# UniHub — Backend

Ecossistema digital para centralizar comércio (Alimentação e Bazar
Acadêmico) e ação social (Ação Solidária) dentro do campus, restrito a
usuários autenticados com e-mail institucional.

## Como o backend funciona

O backend é uma API REST em **.NET 8**, organizada em camadas para separar
regra de negócio de detalhes de infraestrutura:

```
Domain          → entidades puras (Produto, Usuário, Pedido, Doação)
Application     → regras de negócio, interfaces de repositório, DTOs
Infrastructure  → EF Core, acesso ao banco, implementação dos repositórios
API             → Controllers, autenticação, configuração
```

Cada camada só conhece a camada abaixo dela (API → Application →
Infrastructure → Domain), nunca o contrário. Isso permite, por exemplo,
trocar o banco de dados sem alterar uma linha de regra de negócio.

### Autenticação

Todo acesso à API exige um usuário autenticado com e-mail do domínio da
universidade. O login emite um token **JWT**, validado em cada requisição
pelos middlewares configurados no `Program.cs`. Endpoints marcados com
`[Authorize]` rejeitam automaticamente qualquer chamada sem token válido.

### O maior desafio técnico: concorrência no estoque

Durante o intervalo, dezenas de alunos podem tentar comprar o mesmo lanche
ao mesmo tempo. Se o backend apenas lesse o estoque, verificasse em C# e
depois salvasse, existiria uma janela de tempo em que dois pedidos
simultâneos poderiam "ver" a mesma última unidade disponível.

A solução adotada não depende de locks em memória (que não funcionam
quando a API roda em múltiplas instâncias atrás de um load balancer).
Em vez disso, o decremento de estoque é um **UPDATE condicional atômico**
executado diretamente no banco:

```sql
UPDATE Products
SET Stock = Stock - @quantidade
WHERE Id = @id AND Stock >= @quantidade
```

O banco garante que essa operação é atômica. Se duas requisições
concorrentes disputam a última unidade, apenas uma altera uma linha; a
outra recebe zero linhas afetadas e sabe, de forma confiável, que perdeu
a corrida — sem depender de qual instância da API atendeu cada uma.

### Módulos

- **Alimentação:** vendedores publicam produtos com estoque; alunos
  reservam com garantia de consistência mesmo sob alta concorrência.
- **Bazar Acadêmico:** marketplace de compra/venda entre alunos
  (livros, calculadoras, equipamentos), reaproveitando a mesma
  modelagem de produto/estoque, sem a pressão de concorrência extrema.
- **Ação Solidária:** solicitações de doação são pseudonimizadas — a
  identidade real do solicitante fica isolada numa tabela separada,
  acessível apenas por um fluxo administrativo restrito, nunca pelo
  fluxo normal de listagem ou match com voluntários.

## Banco de dados: Neon (Postgres serverless)

O projeto usa **Neon**, um Postgres gerenciado e serverless, no lugar de
uma instância tradicional sempre ligada. Motivos principais:

- **Scale-to-zero:** o banco "dorme" quando não há tráfego (por exemplo,
  fora do horário de aula) e volta a responder em segundos — sem custo
  de compute enquanto ocioso.
- **Branching instantâneo:** cada integrante do time pode criar uma
  branch isolada do banco (com todos os dados copiados) para testar sua
  feature sem afetar o ambiente dos demais.
- **Postgres padrão:** sem vendor lock-in — a mesma modelagem funciona
  se o time decidir migrar para RDS Postgres futuramente.

**Atenção ao usar:**
- Use sempre a *connection string com pooling* (PgBouncer) fornecida pelo
  Neon, não a conexão direta — evita esgotar o limite de conexões.
- O primeiro acesso após um período ocioso pode ter uma latência extra
  (cold start de alguns milissegundos). Isso é especialmente relevante
  antes de rodar o teste de carga de concorrência: vale "aquecer" o
  banco com uma query simples antes de medir.

## Estrutura de pastas

```
UniHub/
├── UniHub.sln
└── src/
    ├── UniHub.Domain/
    │   └── Entities/
    ├── UniHub.Application/
    │   ├── Interfaces/
    │   ├── Services/
    │   └── DTOs/
    ├── UniHub.Infrastructure/
    │   ├── Data/
    │   └── Repositories/
    └── UniHub.API/
        └── Controllers/
```

## Como rodar localmente

1. **Criar um projeto no Neon** (console.neon.tech ou `neonctl`) e copiar
   a connection string com pooling.

2. **Configurar a connection string** em
   `src/UniHub.API/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=<seu-endpoint>.neon.tech;Database=unihub;Username=<user>;Password=<senha>;SSL Mode=Require;Channel Binding=Require;Pooling=true"
     }
   }
   ```

3. **Aplicar as migrations** (dentro de `src/UniHub.API`):
   ```bash
   dotnet ef database update --project ../UniHub.Infrastructure --startup-project .
   ```

4. **Rodar a API:**
   ```bash
   dotnet run --project src/UniHub.API
   ```
   Swagger disponível em `https://localhost:5001/swagger` em ambiente
   Development.

## Status atual / próximos passos

- [x] Estrutura em camadas com injeção de dependência
- [x] Modelagem de entidades dos três módulos
- [x] Resolução de concorrência no estoque (UPDATE condicional)
- [ ] Migrar provider do EF Core de SQL Server para Npgsql (Postgres/Neon)
- [ ] `AuthController` (emissão do JWT após validação do e-mail institucional)
- [ ] Teste automatizado de concorrência (requisições simultâneas)
- [ ] Painel do vendedor e atualização em tempo real (Alimentação)
- [ ] Endpoints do Bazar Acadêmico
- [ ] Serviço de match voluntário-solicitação (Ação Solidária)
