/**
 * Adiciona um listener ao botão de busca de produto.
 * Realiza uma requisição GET para buscar um produto pelo ID informado.
 */
document.getElementById("buscaProduto").addEventListener('click', async(e) =>{
    e.preventDefault();
    const id = document.getElementById('buscaCodigoProduto').value.trim();
    if (id === '') {
        alert('Por favor, preencha o código.');
    } else {
        try {
            const resposta = await fetch(`http://localhost:5121/api/Produtos/${id}`, {
                method: 'GET',
                headers: {'Content-Type': 'application/json'},
                credentials: 'include'
            });

            if (resposta.status === 404) {
                const mensagemErro = await resposta.text(); // Lê o corpo como texto
                alert(mensagemErro)
            }

            if (resposta.ok) {
                const produto = await resposta.json();
                alert(`Produto encontrado\nId: ${produto.id}\nNome: ${produto.nome}\nPreço: R$ ${produto.preco}\nDescrição: ${produto.descricao}\nQtd: ${produto.qtd}`)
            }
                            
        } catch(error){
            console.log(error)
        }
    }
    // Limpa o campo após a busca
    document.getElementById('buscaCodigoProduto').value = "";
})

/**
 * Adiciona um listener ao botão de criação de produto.
 * Realiza uma requisição POST para adicionar um novo produto no banco.
 */
document.getElementById("criarProduto").addEventListener('click', async(e) =>{
    e.preventDefault();

    // Captura e valida os valores dos campos
    const nome = document.getElementById('nome').value.trim();
    const descricao = document.getElementById('descricao').value.trim();
    const preco = document.getElementById('preco').value.trim();
    const qtd = document.getElementById('qtd').value.trim();

    if (nome === '' || descricao === '' || preco === '' || qtd === '') {
        alert('Por favor, preencha todos os dados');
    } else {
        try {
            const produto = {
                "nome": nome, 
                "descricao": descricao, 
                "preco": preco,
                "qtd": qtd
            }

            const resposta = await fetch(`http://localhost:5121/api/Produtos`, {
                method: 'POST',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify(produto),
                credentials: 'include'
            });
            
            if(resposta.status === 400){
                alert('Não foi possível inserir no banco de dados')
            }

            if(resposta.ok){
                const produto = await resposta.json();
                alert(`O produto ${produto.nome} foi inserido com sucesso!`)
            }

        } catch(error){
            console.log(error)
        }
    }

    document.getElementById('nome').value = ''
    document.getElementById('descricao').value = ''
    document.getElementById('preco').value = ''
    document.getElementById('qtd').value = ''
})

/**
 * Adiciona um listener ao botão de exclusão de produto.
 * Realiza uma requisição DELETE para remover um produto pelo ID informado.
 */
document.getElementById("deletarProduto").addEventListener('click', async(e) =>{
    e.preventDefault();
    const id = document.getElementById('codigoProdutoDeletar').value.trim();
    if (id === '') {
        alert('Por favor, preencha o código.');
    } else {
        try {
            const resposta = await fetch(`http://localhost:5121/api/Produtos/${id}`, {
                method: 'DELETE',
                headers: {'Content-Type': 'application/json'},
                credentials: 'include'
            });
                            
            const mensagem = await resposta.text(); // Lê o corpo como texto
            alert(mensagem)

        } catch(error){
            console.log(error)
        }
    }

    document.getElementById('codigoProdutoDeletar').value = "";
})

/**
 * Adiciona um listener ao botão de atualização de produto.
 * Realiza uma requisição PUT para atualizar um campo específico de um produto.
 */
document.getElementById("update").addEventListener('click', async(e) =>{
    e.preventDefault();
    const id = document.getElementById('idAtualizaProduto').value.trim();
    const valor = document.getElementById('valor').value.trim();
    const coluna = document.getElementById('select').value
    
    if (id === '' || valor === '' || coluna === '') {
        alert('Por favor, preencha todos os dados');
    } else {
        try {
            var atualizacao;
            if(coluna == "qtd" || coluna == "preco"){
                atualizacao = {"coluna": coluna, "valor": Number(valor)}
            } else{
                atualizacao = {"coluna": coluna, "valor": valor}
            }

            console.log(atualizacao)
            const resposta = await fetch(`http://localhost:5121/api/Produtos/${id}`, {
                method: 'PUT',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify(atualizacao),
                credentials: 'include'
            });
                            
            if(resposta.status != 201){
                const mensagemErro = await resposta.text(); // Lê o corpo como texto
                alert(mensagemErro)
            }

            if(resposta.ok){
                alert("Atualizado com sucesso!")
            }

        } catch(error){
            console.log(error)
        }
    }

    document.getElementById('idAtualizaProduto').value = ''
    document.getElementById('valor').value = ''
    document.getElementById('select').value = ''
})

/**
 * Adiciona um listener ao botão de listagem de produtos.
 * Realiza uma requisição GET e exibe todos os produtos na tabela HTML.
 */
document.getElementById("listarProduto").addEventListener('click', async(e) =>{
    e.preventDefault();

    // É criada uma tabela que vai sendo incrementada com os valores do banco de dados.
    let inner = `<thead>
        <tr>
            <th>Id</th>
            <th>Nome</th>
            <th>Descrição</th>
            <th>Preço Unitário</th>
            <th>Quantidade</th>
        </tr>
    </thead>
    <tbody>`;

    try {
        const resposta = await fetch(`http://localhost:5121/api/Produtos`, {
            method: 'GET',
            headers: {'Content-Type': 'application/json'},
            credentials: 'include'
        });
                    
        const produtos = await resposta.json();

        // Adiciona cada produto na tabela
        produtos.forEach(produto => {
            inner += `<tr>
                <td>${produto.id}</td>
                <td>${produto.nome}</td>
                <td>${produto.descricao}</td>
                <td>R$ ${produto.preco}</td>
                <td>${produto.qtd}</td>
            </tr>`
        })
        
        inner += `</tbody>`
        document.getElementById('tabelaProduto').innerHTML = inner;
    } catch(error) {
        console.log(error)
        alert('Erro ao carregar o cardápio')
    }
})