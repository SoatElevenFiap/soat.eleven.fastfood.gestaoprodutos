namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.EntityModel.Base;

public interface ITAuditable
{
    public DateTime CriadoEm { get; set; }
    public DateTime ModificadoEm { get; set; }
}
