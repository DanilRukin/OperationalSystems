from package import *
from packet_system import System as sys
from matplotlib import pyplot as plt
import numpy as np
from instruction import *
import random


def shuffle(lst):
    for i in range(len(lst)):
        j = random.randint(0, i)
        lst[i], lst[j] = lst[j], lst[i]


def get_instruction_type():
    if random.randint(0, 100) % 2 == 0:
        return InstructionType.Process
    else:
        return InstructionType.IO

def random_config(sys: sys):
    instructions_count = 10
    packages_count = 5
    instructions = [Instruction((lambda _: InstructionType.IO if random.randint(0, 100) % 2 == 0 else InstructionType.Process)(_),
                                 random.randint(1, 10), sys.logger) for _ in range(instructions_count)]
    # instructions = []
    # for i in range(instructions_count):
    #     instructions.append(Instruction(get_instruction_type(), random.randint(0, 10), sys.logger))
    packages = [Package(sys.logger) for _ in range(packages_count)]
    #packages = list(map(lambda pack: (pack.add_instruction(instructions[i]) for i in range(random.randint(0, instructions_count))), packages))
    for i in range(packages_count):
        num = random.randint(0, instructions_count)
        for j in range(num):
            packages[i].add_instruction(instructions[j])
    for i in range(packages_count):
        sys.add_task(packages[i]) 
    return

def only_proccess_instructions_config(sys: sys):
    instructions_count = 5
    packages_count = 3
    instructions = [Instruction(InstructionType.Process, random.randint(0, 10), sys.logger) for _ in range(instructions_count)]
    packages = [Package(sys.logger) for _ in range(packages_count)]
    for i in range(packages_count):
        num = random.randint(1, instructions_count)
        for _ in range(num):
            packages[i].add_instruction(instructions[random.randint(0, num - 1)])
    for i in range(packages_count):
        sys.add_task(packages[i]) 
    return

def only_io_instructions_config():
    instructions_count = 5
    packages_count = 3
    instructions = [Instruction(InstructionType.IO, random.randint(0, 10), sys.logger) for _ in range(instructions_count)]
    packages = [Package(sys.logger) for _ in range(packages_count)]
    for i in range(packages_count):
        num = random.randint(1, instructions_count)
        for _ in range(num):
            packages[i].add_instruction(instructions[random.randint(0, num - 1)])
    for i in range(packages_count):
        sys.add_task(packages[i]) 
    return

def equal_count_of_io_and_process_instructions_in_package_config(sys: sys):
    instructions_count = 6
    packages_count = 3
    instructions = [Instruction(InstructionType.IO, random.randint(0, 10), sys.logger) for _ in range(instructions_count // 2)]
    instructions.extend([Instruction(InstructionType.Process, random.randint(0, 10), sys.logger) for _ in range(instructions_count // 2)])
    shuffle(instructions)
    packages = [Package(sys.logger) for _ in range(packages_count)]
    for i in range(packages_count):
        num = random.randint(1, instructions_count)
        for _ in range(num):
            packages[i].add_instruction(instructions[random.randint(0, num - 1)])
    for i in range(packages_count):
        sys.add_task(packages[i])
    return