class Event:
    def __init__(self):
        self.__handlers = []

    def subscribe(self, handler):
        self.__handlers.append(handler)

    def unsubscribe(self, handler):
        self.__handlers.remove(handler)

    def invoke(self, sender, event_args):
        for handler in self.__handlers:
            handler(sender, event_args)