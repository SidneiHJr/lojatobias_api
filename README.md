# Introduction

Solução desenvolvida para o desafio backend proposto pela TreeInova.
O banco utilizado no desenvolvimento foi o SqlServer.

# Getting Started
=> É necessário alterar a DefaultConnection de todas as APIs.
=> Comece cadastrando um usuário do tipo Administrador efetuando um post no endpoint api/auth/cadastro
    Pode utilizar o seguinte JSON
    {
        "nome": "Tobias Admin",
        "email": "tobias@gmail.com",
        "perfil": "Administrador",
        "senha": "Teste@123",
        "confirmacaoSenha": "Teste@123"
    }

=> Também é necessário criar um Caixa efetuando um post api/caixa. É necessário estar logado.
    Pode utilizar o seguinte JSON
    {
        "saldoInicial": 50000
    }

=> Cadastre as unidades de medidas ( mg, g, kg, t, etc) antes de cadastrar algum produto pelo post api/produto/unidade-medida
    Segue alguns JSON
    {
        "nome": "Miligrama",
        "abreviacao": "mg"
    }

    {
        "nome": "Grama",
        "abreviacao": "g"
    }

    {
        "nome": "Quilograma",
        "abreviacao": "kg"
    }

    {
        "nome": "Tonelada",
        "abreviacao": "t"
    }

=> É possível comprar/vender um produto com unidades de medidas diferentes. Por exemplo: 
Tenho cadastrado Maça Gala a Granel cuja unidade medida é kg. Porém para vender apenas 200 gramas do produto, 
posso adicionar em um pedido o produto maçã gala a granel só que com a unidade medida em gramas. Com isso é feito uma 
conversão de gramas para kilo, assim o estoque do produto que está em kg será reduzido em 0.2kg.
Para isso é necessário cadastrar as Unidade de Medida Conversao através do post em api/produto/unidade-medida-conversao.
Segue um JSON
    {
        "unidadeMedidaEntradaId": "", //Deve ser o GUID referente a uma unidade medida cadastrada, por exemplo kg
        "unidadeMedidaSaidaId": "", //Deve ser o GUID referente a uma unidade medida cadastrada, por exemplo g
        "fatorConversao": 1000
    } // Nesse caso, será criado uma conversao de kg para g onde irei multiplicar a quantidade por 1000 para ser representado em g.
Lembrando que é necessário criar o caminho inverso também.

=> Nao foi implementado um cadastro de cliente e fornecedor. Foram colocados como string apenas para uma melhor ilustracao.

# Build and Test

Para compra, é necessário criar um pedido de compra e depois finaliza-lo. Foi criado pensando no cenário de que uma compra não é realizada
no momento, demora para chegar. 
    Realizar pedido através de um post em api/pedido/compra
    Finalizar o pedido através de um post em api/pedido/compra/{id}/finalizar


Para venda, é necessário criar um pedido de venda e depois finaliza-lo.
    Realizar pedido através de um post em api/pedido/venda
    Finalizar o pedido através de um post em api/pedido/venda/{id}/finalizar
    
Para ajuste na quantidade do produto, é necessário criar um ajuste.

# Migrations

> cd './src/building blocks/LojaTobias.Infra/'
> dotnet ef migrations add AddInitialMigration -s ../../services/LojaTobias.Identidade.Api/ -p ./ -o ./Data/Migrations
> dotnet ef migrations remove -s ../../services/LojaTobias.Identidade.Api/ -p ./
> dotnet ef database update -s ../../services/LojaTobias.Identidade.Api/ -p ./
