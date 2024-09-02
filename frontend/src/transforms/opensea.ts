export const getEtherscanURL = (address: string) => {
  return `https://etherscan.io/address/${address}`;
};

export const getOpenSeaCollectionURL = (slug: string) => {
  return `https://opensea.io/collection/${slug}`;
};
