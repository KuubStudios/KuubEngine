namespace KuubEngine.Graphics {
    internal interface IBindable {
        int ID { get; }
        void Bind();
        void Unbind();
    }
}