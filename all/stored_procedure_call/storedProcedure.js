function createItems() {}

// This is a pre-trigger function which checks wheather the timestamp key exists in body object or not.
function validateToDoItemTimestamp() {
  let context = getContext();
  let request = getRequest();

  // get the itemToBeCreagted from the body
  let itemToCreate = request.getBody();

  if (!("timeStamp" in itemToCreate)) {
    itemToCreate["timeStamp"] = new Date().getTime();
  }
  // update the item in gthe end
  request.setBody(itemToCreate);
}

// create an item using stored procedures
function createItems(documentToCreate) {
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

function createItem(items) {
  let context = getContext();
  let response = context.getResponse();

  if (!items) {
    response.setBody("Error : Items are not ready to be added.");
    return;
  }

  let numOfItems = items.length;

  checkLength(numItems);
  function checkLength(item) {
    let collection = getContext().getCollection();
    let collectionLink = collection.getSelfLink();

    collection.createDocument(collectionLink, item);
  }
}
