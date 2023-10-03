from package import *
from system import System as sys
from matplotlib import pyplot as plt
import numpy as np
from instruction import *
import random


def config_1(sys: sys):
    instructions_count = 10
    packages_count = 5
    instructions = [Instruction(lambda _: InstructionType.IO if random.randint(0, 100) % 2 == 0 else InstructionType.Process,
                                 random.randint(1, 10)) for _ in range(instructions_count)]
    packages = [Package() for _ in range(packages_count)]
    #packages = list(map(lambda pack: (pack.add_instruction(instructions[i]) for i in range(random.randint(0, instructions_count))), packages))
    for i in range(packages_count):
        num = random.randint(0, instructions_count)
        for j in range(num):
            packages[i].add_instruction(instructions[j])
    for i in range(packages_count):
        sys.add_task(packages[i]) 
    return

def config_2():
    pass

def config_3():
    pass


if __name__ == "__main__":
    os = sys()
    os.configure(config_1)
    os.start(30)
