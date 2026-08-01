' Shared helpers for the hosted OxygenBasic.NET demo.

function Fibonacci(int n) as int
  if n <= 0 then
    return 0
  end if

  if n = 1 then
    return 1
  end if

  return Fibonacci(n - 1) + Fibonacci(n - 2)
end function

function Add(int a, int b) as int
  return a + b
end function
