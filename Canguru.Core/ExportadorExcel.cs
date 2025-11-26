using ClosedXML.Excel;
using System.Collections.Generic;

namespace Canguru.Core
{
    public static class ExportadorExcel
    {
        public static void Exportar(
            string templatePath,
            string savePath,
            List<ResultadoAluno> resultados)
        {
            using var wb = new XLWorkbook(templatePath);
            var ws = wb.Worksheet(1);

            int linha = 2;

            foreach (var r in resultados)
            {
                ws.Cell(linha, 1).Value = r.Aluno.Nome;
                ws.Cell(linha, 2).Value = r.Aluno.Email;
                ws.Cell(linha, 3).Value = r.Acertos;
                ws.Cell(linha, 4).Value = r.Erros;
                ws.Cell(linha, 5).Value = r.Pontos;
                ws.Cell(linha, 6).Value = r.Porcentagem;
                linha++;
            }

            wb.SaveAs(savePath);
        }
    }
}
