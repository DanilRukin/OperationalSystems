from package import *
from packet_system import System as sys
from matplotlib import pyplot as plt
import numpy as np
from instruction import *
import random
import logging
from logging.handlers import TimedRotatingFileHandler
import os


class GlobalParams:
    io_instruction_max_working_time = 3
    io_instruction_min_working_time = 1

    random_config_instructions_count = 10
    random_config_packages_count = 5

def get_logger():
    # определяем формат сообщений
    formatter = logging.Formatter('%(asctime)s %(name)s %(message)s')
    log_file = 'myapp.log'
    if not os.path.exists('logs'):
        os.mkdir('logs')
    log_dir = 'logs'
    logfile = log_dir + '/' + log_file

    # создадим файл логов и обработчик
    fh = TimedRotatingFileHandler(logfile, when='midnight')
    fh.setFormatter(formatter)

    # установим уровень логирования на DEBUG для всех модулей
    ch = logging.StreamHandler()
    ch.setLevel(logging.DEBUG)
    ch.setFormatter(formatter)

    console_level = logging.DEBUG
    file_level = logging.DEBUG

    logger = logging.getLogger()
    logger.setLevel(console_level)
    logger.addHandler(fh)
    logger.addHandler(ch)
    return logger

def shuffle(lst):
    for i in range(len(lst)):
        j = random.randint(0, i)
        lst[i], lst[j] = lst[j], lst[i]


def get_instruction_type():
    if random.randint(0, 100) % 2 == 0:
        return InstructionType.Process
    else:
        return InstructionType.IO
    
def generate_time_for_instruction(instruction_type):
    if instruction_type == InstructionType.Process:
        return random.random()
    elif instruction_type == InstructionType.IO:
        return random.randint(GlobalParams.io_instruction_min_working_time, GlobalParams.io_instruction_max_working_time)
    else:
        raise Exception(f"invalid instruction type: {instruction_type}")

def random_config(sys: sys):
    instructions = []
    for i in range(GlobalParams.random_config_instructions_count):
        type = get_instruction_type()
        instructions.append(Instruction(type, generate_time_for_instruction(type), sys.logger))

    packages = [Package(sys.logger) for _ in range(GlobalParams.random_config_packages_count)]
    #packages = list(map(lambda pack: (pack.add_instruction(instructions[i]) for i in range(random.randint(0, instructions_count))), packages))
    for i in range(GlobalParams.random_config_packages_count):
        num = random.randint(0, GlobalParams.random_config_instructions_count)
        for j in range(num):
            packages[i].add_instruction(instructions[j])
    for i in range(GlobalParams.random_config_packages_count):
        sys.add_task(packages[i]) 
    return

def only_proccess_instructions_config(sys: sys):
    instructions_count = 5
    packages_count = 3
    instructions = [Instruction(InstructionType.Process, generate_time_for_instruction(InstructionType.Process),
                                 sys.logger) for _ in range(instructions_count)]
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
    instructions = [Instruction(InstructionType.IO, generate_time_for_instruction(InstructionType.IO),
                                 sys.logger) for _ in range(instructions_count)]
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
    instructions = [Instruction(InstructionType.IO, generate_time_for_instruction(InstructionType.IO),
                                 sys.logger) for _ in range(instructions_count // 2)]
    instructions.extend([Instruction(InstructionType.Process, generate_time_for_instruction(InstructionType.Process),
                                      sys.logger) for _ in range(instructions_count // 2)])
    shuffle(instructions)
    packages = [Package(sys.logger) for _ in range(packages_count)]
    for i in range(packages_count):
        num = random.randint(1, instructions_count)
        for _ in range(num):
            packages[i].add_instruction(instructions[random.randint(0, num - 1)])
    for i in range(packages_count):
        sys.add_task(packages[i])
    return
