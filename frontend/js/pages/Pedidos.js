// função que busca um pedido
document.getElementById("buscarPedido").addEventListener('click', async(e) =>{
    e.preventDefault();
    const id = document.getElementById('idPedidoBusca').value.trim();
    if (id === '') {
        alert('Por favor, preencha o número do pedido.');

    }else{
        try{
                
            const resposta = await fetch(`http://localhost:5121/api/Pedidos/${id}`, {
                method: 'GET',
                headers: {'Content-Type': 'application/json'},
                credentials: 'include'
            });
                            
            if (resposta.status === 404) {
                const mensagemErro = await resposta.text(); // Lê o corpo como texto
                alert(mensagemErro)
            }

            if (resposta.ok) {
                const pedido = await resposta.json();
                alert(`Pedido encontrado\nId: ${pedido.id}\nNome: ${pedido.nome}\nValor Total: R$ ${pedido.valor.toFixed(2)}`)
            }
    
        } catch(error){
            console.log(error)
        }
    }

    document.getElementById('idPedidoBusca').value = "";
})

// função que insere um pedido

var itens = []

document.getElementById("adicionarProduto").addEventListener('click', async(e) =>{
    e.preventDefault();
    const idProduto = document.getElementById('idProduto').value.trim();
    const qtd = document.getElementById('qtd').value.trim();
    const produto = {"idProduto": idProduto, "qtd": qtd}
    itens.push(produto)

    document.getElementById('idProduto').value = ''
    document.getElementById('qtd').value = ''    
    document.getElementById("nome").disabled = true;
    console.log(itens)
})

document.getElementById("criarPedido").addEventListener('click', async(e) =>{
    e.preventDefault();
    const nome = document.getElementById('nome').value.trim();
    const idProduto = document.getElementById('idProduto').value.trim();
    const qtd = document.getElementById('qtd').value.trim();
    console.log(itens)
    if (nome === '' || qtd === '' || idProduto === '') {
        alert('Por favor, preencha todos os dados');
    }else{
        const produto = {"idProduto": idProduto, "qtd": qtd}

        itens.push(produto)
        try{
            const pedido = {"nomeCliente": nome, "itens": itens}
            console.log(pedido) 
            console.log(itens)   
            const resposta = await fetch(`http://localhost:5121/api/Pedidos`, {
                method: 'POST',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify(pedido),
                credentials: 'include'
            });
                            
            if(resposta.status != 201){
                const mensagemErro = await resposta.text(); // Lê o corpo como texto
                alert(mensagemErro)
            }

            if(resposta.ok){
                const produto = await resposta.json()
                alert(`Inserido com sucesso. Total do Pedido de ${produto.nome} foi de ${produto.valor.toFixed(2)}`)
            }
             
    
        } catch(error){
            console.log(error)
        }
    }
    document.getElementById("nome").disabled = false;
    document.getElementById('nome').value = ''
    document.getElementById('idProduto').value = ''
    document.getElementById('qtd').value = ''
    itens = []
})


// função que lista todos os pedidos

document.getElementById("listAll").addEventListener('click', async(e) =>{
    e.preventDefault();
    inner = `<thead>
    <tr>
        <th>Id</th>
        <th>Nome Cliente</th>
        <th>Valor Total</th>
    </tr>
    </thead>

    <tbody> `;

    const resposta = await fetch(`http://localhost:5121/api/Pedidos`, {
        method: 'GET',
        headers: {'Content-Type': 'application/json'},
        credentials: 'include'
    });
                
    const pedidos = await resposta.json();
    
    pedidos.forEach( pedido =>{
    
        inner += `<tr>
            <td>${pedido.id}</td>
            <td>${pedido.nome}</td>
            <td>R$ ${pedido.valor.toFixed(2)}</td>
        </tr>`
    })
    
    inner += `</tbody>`
    document.getElementById('tabelaPedidos').innerHTML = inner;
})