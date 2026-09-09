# UniHub 🎓

O **UniHub** é um ecossistema digital desenvolvido para centralizar e otimizar as interações socioeconômicas no campus universitário da UNIFESP. O projeto mitiga a fragmentação de informações (dispersas em grupos de WhatsApp e Facebook) através de uma plataforma unificada de alta disponibilidade, dividida em três pilares: comércio de alimentos, revenda de materiais acadêmicos e impacto social.

---

## 🚀 Módulos do Sistema

*   🍔 **Alimentação:** Painel em tempo real para vendedores gerenciarem seus estoques e para alunos realizarem reservas, evitando filas e incertezas durante o intervalo.
*   📚 **Bazar Acadêmico:** Marketplace focado na compra e venda segura de materiais universitários (livros, calculadoras, equipamentos de laboratório e EPIs).
*   🤝 **Ação Solidária:** Módulo dedicado ao apoio social, conectando alunos em vulnerabilidade socioeconômica a voluntários, operando sob rigoroso sigilo de identidade.

---

## 🛠️ Stack Tecnológica e Arquitetura

O sistema foi desenhado com foco em resiliência, escalabilidade horizontal e segurança de dados, utilizando as seguintes tecnologias:

*   **Frontend:** React.js com TypeScript (Single Page Application).
*   **Backend:** API RESTful em C# (.NET Core).
*   **Banco de Dados:** Relacional (SQL Server / PostgreSQL) acessado via Entity Framework Core.
*   **Infraestrutura (Cloud):** AWS (Instâncias EC2 para a aplicação e RDS para o banco de dados).
*   **Integração Contínua:** GitHub Actions para CI/CD.

---

## 🧠 Decisões de Engenharia e Diferenciais Técnicos

Este projeto vai além de um CRUD tradicional, enfrentando desafios reais de engenharia de software:

*   **Controle de Concorrência Otimista (Alta Carga):** Para suportar os picos extremos de requisições durante os intervalos das aulas, o sistema não utiliza bloqueios de *threads* em memória. A concorrência é gerenciada transacionalmente no banco de dados (`UPDATE com validação de linhas afetadas`), garantindo a consistência do estoque mesmo em um ambiente escalado horizontalmente.
*   **Privacy by Design (LGPD):** O módulo de Ação Solidária foi modelado para evitar a re-identificação trivial. A identidade do solicitante é separada da solicitação através de pseudonimização no banco de dados, protegendo dados socioeconômicos sensíveis dos alunos.
*   **Autenticação Institucional:** O acesso à plataforma é estritamente controlado. A emissão do token JWT pelo backend requer validação prévia do domínio institucional (`@unifesp.br`) via link de confirmação, garantindo um ambiente seguro e fechado para a comunidade acadêmica.
*   **Performance:** Utilização de *Connection Pooling* para não esgotar as conexões com o RDS durante picos de acesso.

---

## 👥 Estrutura da Equipe (Scrum Adaptado)

O projeto foi desenvolvido em ciclos ágeis (6 Sprints) por uma equipe técnica de 8 integrantes:

*   **1x Tech Lead / Arquiteto:** Orquestração de rotas, code reviews e padrões de projeto.
*   **2x Desenvolvedores Backend:** Lógica de negócios, API em C# e segurança.
*   **2x Desenvolvedores Frontend:** UI/UX e consumo da API com React.
*   **1x DBA:** Modelagem relacional, *views* e otimização de consultas SQL.
*   **1x DevOps / Cloud:** Provisionamento AWS e CI/CD.
*   **1x Analista de QA:** Testes de integração e testes de carga (concorrência).

---

## ⚙️ Como rodar o projeto localmente

### Pré-requisitos
*   [.NET Core SDK](https://dotnet.microsoft.com/download)
*   [Node.js e npm](https://nodejs.org/)
*   [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) ou Docker para rodar o banco local.

### Passos para execução

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/UniHub-University/Eng.-Software.git
