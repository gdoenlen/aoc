import java.nio.file.Files
import java.util.regex.Pattern
import kotlin.io.path.Path

enum class Operator(private val reducer: (Long, Long) -> Long) {
    PLUS({ acc, next -> acc + next }),
    MULTIPLY({ acc, next -> acc * next });

    fun apply(nums: List<Long>): Long = nums.reduce(this.reducer)
}

class Problem(private val operator: Operator, private val nums: List<Long>) {
    fun calculate() = this.operator.apply(this.nums)
}

fun rotateProblems(lines: List<List<String>>): List<List<String>> {
    val numProblems = lines.first().size - 1
    val rotated = mutableListOf<List<String>>()
    for (i in 0..numProblems) {
        val problem = mutableListOf<String>()
        for (line in lines) {
            problem.add(line[i])
        }

        rotated.add(problem)
    }

    return rotated
}

fun toProblem(line: List<String>): Problem {
    var operator: Operator? = null
    val nums = mutableListOf<Long>()
    for (s in line) {
        when (s) {
            "*" -> operator = Operator.MULTIPLY
            "+" -> operator = Operator.PLUS
            else -> nums.add(s.toLong())
        }
    }

    return Problem(operator!!, nums) // every line has an operator
}

val pattern = Pattern.compile("\\s+")
val lines: List<List<String>> = Files.readAllLines(Path("./input.txt"))
    .map { pattern.split(it).toList() }
val answer = rotateProblems(lines)
    .asSequence()
    .map(::toProblem)
    .sumOf(Problem::calculate)
println(answer)
