function createItems(items) {
  let context = getContext(); // context object provides access to all operations that can be performed in azure cosmos db.
  let response = context.getResponse();

  if (!items) {
    response.setBody("Error : Items are undefined.");
    return;
  }

  let numOfItems = items.length;

  checkLength(numOfItems);
  for (let i = 0; i < numsOfItems; i++) {
    createItem(items[i]);
  }
  function checkLength(length) {
    if (length === 0) {
      response.setBody("Error : There are no items to be added.");
      return;
    }
  }

  function createItem(item) {
    let collection = context.getCollection();
    let collectionLink = collection.getSelfLink();
    collection.createDocument(
      collectionLink,
      item,
      function (err, documentCreated) {
        if (err) {
          throw new Error("Error" + err.message);
        }
        context.getResponse().setBody(documentCreated.id);
      }
    );
  }
}

function createItemThroughStoredProcedure(documentToCreate) {
  let context = getContext();
  let collection = context.getCollection();

  let accepted = collection.createDocument(
    collection.getSelfLink(),
    documentToCreate,
    function (err, documentCreated) {
      if (err) throw new Error("Error " + err.message);
      context.getResponse().setBody(documentCreated.id);
    }
  );
  if (!accepted) return;
}

function validateToDoItemTimestamp() {
  let context = getContext();
  let request = context.getRequest();

  // item to be created in the current operation.
  // get the body here to check the credentials
  let itemToCreate = request.getBody();
  // validate properties
  if (!("timestamps" in itemToCreate)) {
    let ts = new Date();
    itemToCreate["timeStamp"] = ts.getTime();
  }
}
