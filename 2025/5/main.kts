import java.nio.file.Files
import kotlin.io.path.Path

class Ingredients(private val ranges: List<LongRange>) {
  fun isFresh(id: Long) = this.ranges.find { it.contains(id) } != null
}

val ingredients = Ingredients(
  Files.readAllLines(Path("./ranges.txt"))
    .asSequence()
    .map(::toRange)
    .toList()
)

fun toRange(line: String): LongRange {
  val parts = line.split("-")
  val start = parts[0].toLong()
  val end = parts[1].toLong()

  return start..end
}

val sum = Files.readAllLines(Path("./ids.txt"))
  .asSequence()
  .map { it.toLong() }
  .filter(ingredients::isFresh)
  .count()
println(sum)
