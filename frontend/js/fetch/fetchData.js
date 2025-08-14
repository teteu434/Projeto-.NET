
export async function fetchData(rota) {
    try {
        const response = await fetch(`http://localhost:4000/${rota}`);
        const dados = await response.json();
        return dados;
        
    } catch (error) {
        console.error("Erro ao buscar dados:", error);
        return [];
    }
}