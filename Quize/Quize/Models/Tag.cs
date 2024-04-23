using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


// Tag model
public class Tag { 

    public Tag(){
        QuizList = new HashSet<Quiz>();
    }


    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<Quiz> QuizList { get; set; }


}