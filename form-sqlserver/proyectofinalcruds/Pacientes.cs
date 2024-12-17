using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace proyectofinalcruds
{
    public partial class Pacientes : Form
    {
        // String de conexión a la base de datos
        public string conexionBD = @"Server=FATY;Database=ClinicaDB;Integrated Security=True";

        public Pacientes()
        {
            InitializeComponent();
            CargarPacientes();
        }

        // Evento para el botón Regreso
        private void btnRegreso_Click(object sender, EventArgs e)
        {
            // Crear una instancia del formulario Menu
            Menu menuPrincipal = new Menu();

            // Mostrar el nuevo formulario
            menuPrincipal.Show();

            // Ocultar el formulario actual
            this.Hide();
        }

        // Método para cargar los datos de la tabla en el DataGridView
        private void CargarPacientes()
        {
            using (SqlConnection conexion = new SqlConnection(conexionBD))
            {
                string query = "SELECT Id, Nombre, Apellido, Sexo, Edad, Fecha_Creacion, Estado FROM pacientes";
                SqlDataAdapter da = new SqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPacientes.DataSource = dt;
            }
        }

        // Evento para cargar los datos al abrir el formulario
        private void Pacientes_Load(object sender, EventArgs e)
        {
            CargarPacientes();
        }


        // Evento para el botón Insertar
        private void btnInsertar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexion = new SqlConnection(conexionBD))
            {
                string query = "INSERT INTO pacientes (Nombre, Apellido, Sexo, Edad, Usuario_Creador, Fecha_Creacion, Estado) " +
                               "VALUES (@Nombre, @Apellido, @Sexo, @Edad, @Usuario_Creador, @Fecha_Creacion, @Estado)";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Nombre", txtNombres.Text);
                cmd.Parameters.AddWithValue("@Apellido", txtApellidos.Text);
                cmd.Parameters.AddWithValue("@Sexo", cmbxSexo.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Edad", txtEdad.Text);
                cmd.Parameters.AddWithValue("@Usuario_Creador", 1); // Aquí puedes cambiar por el valor adecuado
                cmd.Parameters.AddWithValue("@Fecha_Creacion", DateTime.Now);
                cmd.Parameters.AddWithValue("@Estado", "Activo");

                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();

                MessageBox.Show("Paciente insertado correctamente.");
                CargarPacientes();
            }
        }

        // Evento para el botón Actualizar
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexion = new SqlConnection(conexionBD))
            {
                string query = "UPDATE pacientes SET Nombre = @Nombre, Apellido = @Apellido, Sexo = @Sexo, Edad = @Edad, " +
                               "Usuario_Modificador = @Usuario_Modificador, Fecha_Modificacion = @Fecha_Modificacion, Estado = @Estado " +
                               "WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Nombre", txtNombres.Text);
                cmd.Parameters.AddWithValue("@Apellido", txtApellidos.Text);
                cmd.Parameters.AddWithValue("@Sexo", cmbxSexo.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Edad", txtEdad.Text);
                cmd.Parameters.AddWithValue("@Usuario_Modificador", 1); // Aquí puedes cambiar por el valor adecuado
                cmd.Parameters.AddWithValue("@Fecha_Modificacion", DateTime.Now);
                cmd.Parameters.AddWithValue("@Estado", "Activo");
                cmd.Parameters.AddWithValue("@Id", dgvPacientes.CurrentRow.Cells["Id"].Value); // Usamos el Id de la fila seleccionada

                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();

                MessageBox.Show("Paciente actualizado correctamente.");
                CargarPacientes();
            }
        }

        // Evento para el botón Eliminar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPacientes.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvPacientes.CurrentRow.Cells["Id"].Value);

                using (SqlConnection conexion = new SqlConnection(conexionBD))
                {
                    string query = "DELETE FROM pacientes WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Id", id);

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    conexion.Close();
                }

                MessageBox.Show("Paciente eliminado correctamente.");
                CargarPacientes();
            }
            else
            {
                MessageBox.Show("Seleccione un paciente para eliminar.");
            }
        }

        private void dgvPacientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica si se ha hecho clic en una fila válida (no en el encabezado)
            if (e.RowIndex >= 0)
            {
                // Obtiene la fila seleccionada
                DataGridViewRow row = dgvPacientes.Rows[e.RowIndex];

                // Carga los valores de la fila seleccionada en los controles correspondientes
                txtNombres.Text = row.Cells["Nombre"].Value.ToString();
                txtApellidos.Text = row.Cells["Apellido"].Value.ToString();
                cmbxSexo.SelectedItem = row.Cells["Sexo"].Value.ToString();
                txtEdad.Text = row.Cells["Edad"].Value.ToString();
            }
        }


        
    }
}
