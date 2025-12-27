# Primeira precisa criar a infraestrutura

# Segundo precisa logar no azure e no ACR

az login
az aks get-credentials --resource-group rg-fastfood-postech --name aks-fastfood-postech --overwrite-existing
az acr login --name acrfastfoodpostech

# Terceiro precisa buildar as imagens e jogar para o ACR

docker build --target app -t acrfastfoodpostech.azurecr.io/fastfood/app:v1 .
docker push acrfastfoodpostech.azurecr.io/fastfood/app:v1

docker build --target migrator -t acrfastfoodpostech.azurecr.io/fastfood/migrator:v2 .
docker push acrfastfoodpostech.azurecr.io/fastfood/migrator:v2


# criei uma secret para acessar o acr

kubectl create namespace fastfood
$ACR_USER = az acr credential show -n acrfastfoodpostech --query username -o tsv
$ACR_PASS = az acr credential show -n acrfastfoodpostech --query passwords[0].value -o tsv
$ACR_SERVER = "acrfastfoodpostech.azurecr.io"

kubectl create secret docker-registry acr-secret `
  --namespace fastfood `
  --docker-server=$ACR_SERVER `
  --docker-username=$ACR_USER `
  --docker-password=$ACR_PASS


# Verificar o ID do Gateway
az network application-gateway show --resource-group rg-fastfood-postech --name agw-fastfood-postech --query id --output tsv

# Colar o resultado do comando passado

az aks enable-addons --resource-group rg-fastfood-postech --name aks-fastfood-postech --addons ingress-appgw --appgw-id <Cole aqui>

exemplo: az aks enable-addons --resource-group rg-fastfood-postech --name aks-fastfood-postech --addons ingress-appgw --appgw-id "/subscriptions/09d5a929-8c74-463b-bf47-678505201104/resourceGroups/rg-fastfood-postech/providers/Microsoft.Network/applicationGateways/agw-fastfood-postech" 

# Comando para vincular o AKS ao ACR e dar a permissão para realizar  Depois de vinculado, o cluster AKS terá permissão para puxar imagens do ACR automaticamente, e o Helm funcionará normalmente para deploys que usam imagens desse repositório. Só será necessário rodar novamente se você trocar de ACR ou criar um novo cluster.
az aks update -n aks-fastfood-postech -g rg-fastfood-postech --attach-acr acrfastfoodpostech

# Comando para instalar o helm
helm install fastfood-release helm/fastfood-chart --namespace fastfood

# Comando para pegar o ip 
k get ingress -n fastfood
exemplo de retorno
NAME               CLASS    HOSTS   ADDRESS         PORTS   AGE
fastfood-ingress   <none>   *       52.143.181.75   80      48s


colocar no arquivo hosts o ip com o dns fastfood
caminho do arquivo
"C:/Windows/System32/drivers/etc/hosts"
exemplo de como ficaria e
52.143.181.75 fastfood

cuidado ao salvar o arquivo lembra que o bloco de notas precisa estar com acesso admnistrador e não salvar como o tipo de documento de texto e sim como todos os arquivos

# Comando para desistalar o helm
helm uninstall fastfood-release --namespace fastfood   
